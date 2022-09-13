using Core.Database;
using Core.DynamicProperties.Services;
using Core.Exceptions;
using Core.ObjectMapping;
using Membership.Core;
using Membership.Core.Contracts;
using Membership.Core.Dto;
using Membership.Core.Entities;
using Membership.Core.Exceptions;
using Membership.Library.Contracts;
using Membership.Library.Data;
using Membership.Library.Dto;
using Membership.Library.Dto.Customer;
using Membership.Library.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Membership.Library.Services
{
    public class CustomersService : BaseService<Company, CompanyDto>, ICustomersService
    {
        private readonly IMembership membership;
        private readonly IRepository repository;
        private readonly ICustomerAuditService customerAuditService;
        private readonly ICompanyService companyService;
        private readonly IDynamicPropertyValueService dynamicPropertyValueService;
        private readonly IUserService _userService;

        public CustomersService(IMembership membership,
                                IRepository repository,
                                ICustomerAuditService customerAuditService,
                                IMappingService mappingService,
                                ICompanyService companyService,
                                IDynamicPropertyValueService dynamicPropertyValueService, IUserService userService)
            : base(mappingService, repository)
        {
            this.membership = membership;
            this.repository = repository;
            this.customerAuditService = customerAuditService;
            this.companyService = companyService;
            this.dynamicPropertyValueService = dynamicPropertyValueService;
            _userService = userService;
        }


        public IQueryable<Company> GetAllCustomers()
        {
            var query = membership.IsSysAdmin ? GetAll() : Fetch(x => x.MasterCompany != null);

            var bizOwner = membership.GetCurrentBizOwner();
            if (bizOwner != null)
            {
                query = query.Where(c => c.MasterCompany.Id == bizOwner.Id);
            }

            return query;
        }

        public IQueryable<CompanyAutocompleteDto> GetAllAutocomplete()
        {
            var companyAutocompleteDtos =
                GetAllCustomers()
                    .Where(c => !c.IsDeleted && (c.MainContactUser == null || !c.MainContactUser.IsDeleted))
                    .ToArray();

            var result = MappingService.Map<List<CompanyAutocompleteDto>>(companyAutocompleteDtos);

            return result.AsQueryable();
        }

        public Company CreateCustomer(CompanyDto model)
        {
            var company = Create(model);

            company.MasterCompany = membership.GetCurrentBizOwner();

            repository.Update(company);
            repository.SaveChanges();

            if (model.MainContactUser != null)
            {
                try
                {
                    // company client's company will be the customer.
                    model.MainContactUser.IsMainContactUser = true;
                    model.MainContactUser.CompanyId = company.Id;

                    SaveContact(model.MainContactUser);
                }
                catch (UserEmailAlreadyInUsedException exception)
                {
                    repository.HardDelete(company);
                    repository.SaveChanges();
                    throw;
                }
                catch (UserPhoneNumberAlreadyInUsedException exception)
                {
                    repository.HardDelete(company);
                    repository.SaveChanges();
                    throw;
                }
            }

            return company;
        }

        public void Revert(long id)
        {
            var company = repository.Get<Company>(id);
            company.IsDeleted = false;
            repository.Update(company);
            repository.SaveChanges();
        }

        public object GetCustomerDetails(long companyId)
        {
            CompanyDto dto = companyService.GetDto(companyId);

            if (dto.IsDeleted)
            {
                return null;
            }

            dto.ExtendedProperties = dynamicPropertyValueService.GetExtendedPropertyValuesForEntity<Company>(dto.Id);

            customerAuditService.LogCustomerViewed(dto.Id, membership.UserId);

            return dto;

            /*var customer = new
            {
                Id = company.Id,
                Name = company.CompanyName,
                Rep = source.rep != null ? string.Format("{0} {1}", source.rep.fname, source.rep.lname) : string.Empty,
                Priority = source.customer_priority,
                Address = new
                {
                    Street = source.street.Trim(),
                    City = source.city.Trim(),
                    State = source.state.Trim(),
                    ZipCode = source.zipcode
                }
            };*/



            //return customer;

            //return null;

            /*var source = this.asiDbRepository.GetAll<MagTac.Data.Entities.AsiDb.Customer>().First(x => x.id == customerId);

            var customer = new
            {
                Id = source.id,
                Name = source.name.Trim(),
                Rep = source.rep != null ? string.Format("{0} {1}", source.rep.fname, source.rep.lname) : string.Empty,
                Priority = source.customer_priority,
                Address = new
                {
                    Street = source.street.Trim(),
                    City = source.city.Trim(),
                    State = source.state.Trim(),
                    ZipCode = source.zipcode
                }
            };

            this.customerAuditService.LogCustomerViewed(customerId, this.membership.UserId);

            return customer;*/
        }

        /*public ConnectionInfoDto GetConnectionInfo(int customerId)
        {
            var customer =
                this.asiDbRepository.GetAll<MagTac.Data.Entities.AsiDb.Customer>()
                    .FirstOrDefault(x => x.id == customerId);

            var dto = new ConnectionInfoDto
            {
                Id = customerId,
                Name = customer.name.Trim()
            };

            var connectionInfoOverride =
                this.repository.GetAll<ConnectionInfoOverride>().FirstOrDefault(x => x.CustomerId == customerId);

            if (connectionInfoOverride != null)
            {
                dto.Content = connectionInfoOverride.Content;
            }
            else
            {
                dto.Content = customer.connect;
            }

            this.customerAuditService.LogCustomerViewed(customerId, this.membership.UserId);

            return dto;
        }

        public void SaveConnectionInfo(ConnectionInfoDto dto)
        {
            var connectionInfoOverride =
                this.repository.GetAll<ConnectionInfoOverride>().FirstOrDefault(x => x.CustomerId == dto.Id);

            if (connectionInfoOverride == null)
            {
                connectionInfoOverride = new ConnectionInfoOverride
                {
                    CustomerId = dto.Id,
                    Content = dto.Content
                };

                this.repository.Insert(connectionInfoOverride);
            }
            else
            {
                connectionInfoOverride.Content = dto.Content;
                this.repository.Update(connectionInfoOverride);
            }

            this.repository.SaveChanges();
        }*/

        #region Contacts

        public IQueryable<CustomerContact> QueryCustomerContacts()
        {
            return repository.GetAll<CustomerContact>();
        }

        public List<UserBaseModel> GetContacts(long companyId)
        {
            var company = repository.Get<Company>(companyId);

            if (company == null)
                throw new BaseNotFoundException<Company>();

            var contactUsers =
                repository.Fetch<User>(
                    user => !user.IsDeleted && (user.CompanyId == companyId || user.ClientCompanyId == companyId));

            var contactUsersDto = MappingService.Map<List<UserBaseModel>>(contactUsers);

            var mainUser = contactUsersDto.FirstOrDefault(user => user.Id == company.MainContactUser.Id);

            if (mainUser != null)
                mainUser.IsMainContactUser = true;

            return contactUsersDto;
        }

        public void SaveContact(UserBaseModel contact)
        {
            long mainContactUser = contact.Id;
            if (contact.Id != 0)
            {
                var user = _userService.GetUser(contact.Id);
                MappingService.Map(contact, user);
                repository.Update(user);
                repository.SaveChanges();
            }
            else
            {
                if (contact.Roles == null)
                    contact.Roles = new List<string>();

                if (!contact.Roles.Contains(MembershipConstant.C_ROLE_CODE_COMPANYCLIENT))
                    contact.Roles.Add(MembershipConstant.C_ROLE_CODE_COMPANYCLIENT);

                if (!contact.Roles.Contains(MembershipConstant.C_ROLE_CODE_COMPANYCLIENTSTAFF))
                    contact.Roles.Add(MembershipConstant.C_ROLE_CODE_COMPANYCLIENTSTAFF);

                var user = _userService.Register(contact);
                mainContactUser = user.Id;
            }

            if (contact.IsMainContactUser)
            {
                var customer = repository.Get<Company>(contact.CompanyId);
                if (customer == null)
                    return;

                customer.MainContactUser = _userService.GetUser(mainContactUser);
                repository.Update(customer);
                repository.SaveChanges();
            }
        }

        public void DeleteContact(long contactId)
        {
            var contactUser = repository.Get<User>(contactId);

            if (contactUser == null || contactUser.IsDeleted)
            {
                throw new BaseNotFoundException<User>();
            }

            repository.Delete(contactUser);
            repository.SaveChanges();
        }

        #endregion

        #region Sites

        public List<SiteDto> GetSites(long companyId)
        {
            return repository
                .GetAll<CustomerSite>()
                .Where(x => x.Company.Id == companyId)
                .ToList()
                .Select(s => new SiteDto
                {
                    Id = s.Id,
                    CompanyId = s.Company.Id,
                    Name = s.Name,
                    Street = s.Street,
                    City = s.City,
                    State = s.State,
                    ZipCode = s.Zipcode
                }).ToList();
        }

        public void SaveSite(SiteDto dto)
        {
            CustomerSite site;

            if (dto.Id != 0)
            {
                site = repository.GetAll<CustomerSite>().FirstOrDefault(x => x.Id == dto.Id);
            }
            else
            {
                site = new CustomerSite();
                site.Company = repository.GetAll<Company>().FirstOrDefault(x => x.Id == dto.CompanyId);
            }

            site.Name = dto.Name;
            site.Street = dto.Street;
            site.Street2 = dto.Street2;
            site.City = dto.City;
            site.State = dto.State;
            site.Zipcode = dto.ZipCode;

            if (dto.Id != 0)
            {
                repository.Update(site);
            }
            else
            {
                repository.Insert(site);
            }

            repository.SaveChanges();
        }

        public void DeleteSite(SiteDto dto)
        {
            var site = repository.GetAll<CustomerSite>().FirstOrDefault(x => x.Id == dto.Id);

            if (site != null)
            {
                repository.Remove(site);
                repository.SaveChanges();
            }
        }

        #endregion

        public object GetDocuments(long companyId)
        {
            return repository.GetAll<CompanyDocument>()
                .Where(x => x.Master.Id == companyId)
                .ToList()
                .Select(d => new
                {
                    Name = d.Comment,
                    Date = d.ModifiedDate
                }).ToList();
        }

        #region Partnerships

        public List<PartnershipDto> GetPartnerships(long companyId)
        {
            return repository
                .GetAll<CustomerPartner>()
                .Where(x => x.Company.Id == companyId)
                .ToList()
                .Select(cp => new PartnershipDto
                {
                    Id = cp.Id,
                    CompanyId = cp.Company.Id,
                    PartnerId = cp.Partner.Id,
                    Name = cp.Partner.Name,
                    Type = cp.Partner.Type
                }).ToList();
        }

        public void SavePartnership(PartnershipDto dto)
        {
            var partner = repository.GetAll<Partner>().FirstOrDefault(x => x.Id == dto.PartnerId);

            if (dto.Id != 0)
            {
                var customerPartner = repository.GetAll<CustomerPartner>().FirstOrDefault(x => x.Id == dto.Id);
                customerPartner.Partner = partner;
                repository.Update(customerPartner);
            }
            else
            {
                var customerPartner = new CustomerPartner();

                customerPartner.Company = repository.GetAll<Company>().FirstOrDefault(x => x.Id == dto.CompanyId); ;
                customerPartner.Partner = partner;

                repository.Insert(customerPartner);
            }

            repository.SaveChanges();
        }

        public void DeletePartnership(PartnershipDto dto)
        {
            var customerPartner = repository.GetAll<CustomerPartner>().FirstOrDefault(x => x.Id == dto.Id);

            if (customerPartner != null)
            {
                repository.Remove(customerPartner);
                repository.SaveChanges();
            }
        }

        #endregion

        #region Vendors

        public List<VendorDto> GetCustomerVendors(long companyId)
        {
            var source = repository
                .GetAll<CustomerVendor>()
                .Where(x => x.Company.Id == companyId)
                .ToList();

            var result = new List<VendorDto>();

            foreach (var x in source)
            {
                var dto = new VendorDto
                {
                    Id = x.Id,
                    CompanyId = x.Company.Id,
                    Notes = x.Notes
                };

                if (x.Site != null)
                {
                    dto.SiteId = x.Site.Id;
                }

                if (x.Vendor != null)
                {
                    dto.VendorId = x.Vendor.Id;
                    dto.Name = x.Vendor.Name;
                }

                if (x.VendorType != null)
                {
                    dto.VendorTypeId = x.VendorType.Id;
                    dto.Type = x.VendorType.TypeName;
                }

                if (x.ProjectDescription != null)
                {
                    dto.ProjectDescriptionId = x.ProjectDescription.Id;
                    dto.ProjectDescription = x.ProjectDescription.Description;
                }

                result.Add(dto);
            }

            return result;
        }

        public void SaveCustomerVendor(VendorDto dto)
        {
            CustomerVendor customerVendor;

            if (dto.Id != 0)
            {
                customerVendor = repository.GetAll<CustomerVendor>().FirstOrDefault(x => x.Id == dto.Id);
            }
            else
            {
                customerVendor = new CustomerVendor();
                customerVendor.Company = repository.GetAll<Company>().FirstOrDefault(x => x.Id == dto.CompanyId);
            }

            customerVendor.Vendor = repository.GetAll<Vendor>().FirstOrDefault(x => x.Id == dto.VendorId);
            customerVendor.VendorType = repository.GetAll<VendorType>().FirstOrDefault(x => x.Id == dto.VendorTypeId);
            customerVendor.ProjectDescription = repository.GetAll<ProjectDescription>().FirstOrDefault(x => x.Id == dto.ProjectDescriptionId);
            customerVendor.Site = repository.GetAll<CustomerSite>().FirstOrDefault(x => x.Id == dto.SiteId);
            customerVendor.Notes = dto.Notes;

            if (dto.Id != 0)
            {
                repository.Update(customerVendor);
            }
            else
            {
                repository.Insert(customerVendor);
            }

            repository.SaveChanges();
        }

        public void DeleteCustomerVendor(VendorDto dto)
        {
            var customerVendor = repository.GetAll<CustomerVendor>().FirstOrDefault(x => x.Id == dto.Id);

            if (customerVendor != null)
            {
                repository.Remove(customerVendor);
                repository.SaveChanges();
            }
        }

        #endregion

        #region Lookups

        public List<LookupDto> GetPartners()
        {
            return repository
                .GetAll<Partner>()
                .Select(x => new LookupDto { Id = x.Id, Name = x.Name, ExternalId = x.ExternalId })
                .ToList();
        }

        public void SavePartner(PartnershipDto dto)
        {
            var partner = repository.GetAll<Partner>().FirstOrDefault(x => x.ExternalId == dto.ExternalId);
            bool isCreate = false;

            if (partner == null)
            {
                partner = new Partner();
                isCreate = true;
            }

            partner.Name = dto.Name;
            partner.Type = dto.Type;
            partner.ExternalId = dto.ExternalId;

            if (isCreate)
            {
                repository.Insert(partner);
            }
            else
            {
                repository.Update(partner);
            }

            repository.SaveChanges();
        }

        public List<LookupDto> GetVendors()
        {
            return repository
                .GetAll<Vendor>()
                .Select(x => new LookupDto { Id = x.Id, Name = x.Name, ExternalId = x.ExternalId })
                .ToList();
        }

        public void SaveVendor(LookupDto dto)
        {
            var vendor = repository.GetAll<Vendor>().FirstOrDefault(x => x.ExternalId == dto.ExternalId);
            bool isCreate = false;

            if (vendor == null)
            {
                vendor = new Vendor();
                isCreate = true;
            }

            vendor.Name = dto.Name;
            vendor.ExternalId = dto.ExternalId;

            if (isCreate)
            {
                repository.Insert(vendor);
            }
            else
            {
                repository.Update(vendor);
            }

            repository.SaveChanges();
        }

        public List<LookupDto> GetVendorTypes()
        {
            return repository
                .GetAll<VendorType>()
                .Select(x => new LookupDto { Id = x.Id, Name = x.TypeName, ExternalId = x.ExternalId })
                .ToList();
        }

        public void SaveVendorType(LookupDto dto)
        {
            var vendorType = repository.GetAll<VendorType>().FirstOrDefault(x => x.ExternalId == dto.ExternalId);
            bool isCreate = false;

            if (vendorType == null)
            {
                vendorType = new VendorType();
                isCreate = true;
            }

            vendorType.TypeName = dto.Name;
            vendorType.ExternalId = dto.ExternalId;

            if (isCreate)
            {
                repository.Insert(vendorType);
            }
            else
            {
                repository.Update(vendorType);
            }

            repository.SaveChanges();
        }

        public List<LookupDto> GetProjectDescriptions()
        {
            return repository
                .GetAll<ProjectDescription>()
                .Select(x => new LookupDto { Id = x.Id, Name = x.Description, ExternalId = x.ExternalId })
                .ToList();
        }

        public void SaveProjectDescription(LookupDto dto)
        {
            var projectDescription = repository.GetAll<ProjectDescription>().FirstOrDefault(x => x.ExternalId == dto.ExternalId);
            bool isCreate = false;

            if (projectDescription == null)
            {
                projectDescription = new ProjectDescription();
                isCreate = true;
            }

            projectDescription.Description = dto.Name;
            projectDescription.ExternalId = dto.ExternalId;

            if (isCreate)
            {
                repository.Insert(projectDescription);
            }
            else
            {
                repository.Update(projectDescription);
            }

            repository.SaveChanges();
        }

        #endregion
    }
}
