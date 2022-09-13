using Core.Database;
using Core.DynamicProperties.Services;
using Core.Exceptions;
using Core.ObjectMapping;
using Membership.Core.Entities;
using Membership.Core.Entities.Enums;
using Membership.Library.Contracts;
using Membership.Library.Data;
using Membership.Library.Dto;
using Microsoft.Web.Administration;
using System;
using System.Configuration;
using System.Linq;

namespace Membership.Library.Services
{
    public class CompanyService : BaseService<Company, CompanyDto>, ICompanyService
    {
        private readonly IDynamicPropertyValueService _dynamicPropertyValueService;

        private readonly string _baseDomain = ConfigurationManager.AppSettings["PORTAL_BASE_DOMAIN"];

        public CompanyService(IDynamicPropertyValueService dynamicPropertyValueService,
                              IMappingService mappingService,
                              IRepository membershipRepository)
            : base(mappingService, membershipRepository)
        {
            _dynamicPropertyValueService = dynamicPropertyValueService;
        }

        public void AssignSubDomain(string subdomain)
        {
            var domain = subdomain + "." + _baseDomain;

            AddHostHeader(domain);
        }

        public void InsertOrUpdateCompany(string key, string name)
        {
            var company = Repository.First<Company>(x => x.Code == key);

            if (company == null)
                Repository.Insert(new Company
                {
                    Code = key,
                    Name = name
                });
            else
                company.Name = name;

            Repository.SaveChanges();
        }

        public void InsertOrUpdateCompany(CompanyDto dto)
        {
            var company = dto.Id == 0 ? new Company() : Repository.Get<Company>(dto.Id);

            MappingService.Map(dto, company);

            if (dto.Id == 0)
                Repository.Insert(company);
            else
                Repository.Update(company);

            Repository.SaveChanges();

            _dynamicPropertyValueService.UpdateEntityAdditionalFields<Company>(company.Id, dto.ExtendedProperties);
        }

        public void UpdateCompanyStatus(long id, CompanyStatus status, DateTime? statusValidDate)
        {
            var company = Repository.Get<Company>(id);

            company.Status = status;
            company.StatusLastUpdated = DateTime.UtcNow;
            company.StatusValidDate = statusValidDate;

            Repository.Update(company);
            Repository.SaveChanges();
        }

        public void CreateCompanyForUser(CompanyDto companyModel, long userId)
        {
            var masterCompany = Repository.First<Company>();
            if (masterCompany == null)
                throw new InvalidOperationException("The site is not configured correctly");

            var user = Repository.Get<User>(userId);

            if (user == null)
                throw new BaseNotFoundException<User>();

            var companyEntity = MappingService.Map<Company>(companyModel);

            companyEntity.Owner = user;
            companyEntity.MainContactUser = user;
            companyEntity.MasterCompanyId = masterCompany.Id;
            user.Company = companyEntity;
            Repository.Insert(companyEntity);
            Repository.Update(user);
            Repository.SaveChanges();

            _dynamicPropertyValueService.UpdateEntityAdditionalFields<Company>(companyEntity.Id, companyModel.ExtendedProperties);
        }

        private void AddHostHeader(string hostHeader)
        {
            var serverManager = new ServerManager();
            var website =
                serverManager.Sites.FirstOrDefault(
                                 s =>
                                     s.Bindings.Any(
                                          b =>
                                              b.Host.Equals(_baseDomain,
                                                            StringComparison.InvariantCultureIgnoreCase) &&
                                              (b.EndPoint.Port == 80)));
            if (website == null)
                throw new /*NotFound*/ Exception("Site with domain " + _baseDomain + " not found");

            if (website.Bindings.Any(b => b.Host.Equals(hostHeader, StringComparison.InvariantCultureIgnoreCase)))
                return;


            website.Bindings.Add(string.Format(":80:{0}", hostHeader), "http");

            // commit
            serverManager.CommitChanges();
        }
    }
}