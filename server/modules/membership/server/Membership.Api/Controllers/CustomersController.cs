using Core.ObjectMapping;
using Core.UI;
using Core.UI.DataTablesExtensions;
using Core.WebApi.ActionFilters;
using DTWrapper.Core;
using Membership.Api.Controllers.Base;
using Membership.Api.Models;
using Membership.Core;
using Membership.Core.Contracts;
using Membership.Core.Contracts.AuthAttributes;
using Membership.Core.Dto;
using Membership.Core.Entities;
using Membership.Core.Exceptions;
using Membership.Library.Contracts;
using Membership.Library.Dto;
using Membership.Library.Dto.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Membership.Api.Controllers
{
    [RequireAuthTokenApi]
    [ErrorResponseHandler(typeof(UserEmailAlreadyInUsedException), HttpStatusCode.BadRequest)]
    public class CustomersController : DTApiController<CompanyShortDto>
    {
        private readonly ISmallCache _cache;
        //private readonly ICompanyService companyService;
        private readonly ICustomerAuditService _customerAuditService;
        private readonly ICustomersService _customersService;
        private readonly IDataTableService _datatableService;
        private readonly IMembership _membership;
        private readonly IUserService _userService;
        private readonly IMappingService _mappingService;

        public CustomersController(IMembership membership, ICustomersService customersService,
                                   ICustomerAuditService customerAuditService, IUserService userService,
                                   ISmallCache cache, IDataTableService datatableService, IMappingService mappingService)
        {
            _membership = membership;
            _customersService = customersService;
            _customerAuditService = customerAuditService;
            _userService = userService;
            _cache = cache;
            _datatableService = datatableService;
            _mappingService = mappingService;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            var companies = _customersService.GetAllAutocomplete().ToList();

            var recentIds = _customerAuditService.GetRecentCustomers(_membership.UserId);
            recentIds.Reverse();

            foreach (var id in recentIds)
            {
                var item = companies.FirstOrDefault(x => x.Id == id);
                if (item != null)
                {
                    companies.Remove(item);
                    companies.Insert(0, item);
                }
            }

            return
                Json(
                    companies.Select(
                        x =>
                        new
                        {
                            id = x.Id,
                            value =
                            string.Format("{0} {1} - {2}", x.Name,
                                          string.IsNullOrEmpty(x.Alias) ? string.Empty : " - " + x.Alias, x.Code)
                        }));
        }

        [HttpPost]
        public HttpResponseMessage Create(CompanyDto model)
        {
            _customersService.CreateCustomer(model);
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpPut]
        public HttpResponseMessage Update(long id, CompanyDto model)
        {
            _customersService.Update(model, id);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpDelete]
        public HttpResponseMessage Delete(long id)
        {
            _customersService.Delete(id);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage RevertCustomer(long id)
        {
            _customersService.Revert(id);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpGet]
        public IHttpActionResult GetDetails(int id)
        {
            return Ok(_customersService.GetCustomerDetails(id));
        }

        [HttpGet]
        public IHttpActionResult Contacts(int id)
        {
            return Ok(_customersService.GetContacts(id));
        }

        [HttpPost]
        public IHttpActionResult SaveContact(UserBaseModel model)
        {
            _customersService.SaveContact(model);

            return Ok();
        }

        [HttpPost, Route("deleteContact/{contactId:long}")]
        public IHttpActionResult DeleteContact(long contactId)
        {
            _customersService.DeleteContact(contactId);

            return Ok();
        }

        [HttpGet]
        public IHttpActionResult Sites(int id)
        {
            return Ok(_customersService.GetSites(id));
        }

        [HttpPost]
        public IHttpActionResult SaveSite(SiteDto model)
        {
            _customersService.SaveSite(model);

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult DeleteSite(SiteDto model)
        {
            _customersService.DeleteSite(model);

            return Ok();
        }

        [HttpGet]
        public IHttpActionResult Documents(int id)
        {
            return Ok(_customersService.GetDocuments(id));
        }

        [HttpGet]
        public IHttpActionResult Vendors(int id)
        {
            return Ok(_customersService.GetCustomerVendors(id));
        }

        [HttpGet]
        public IHttpActionResult Partnerships(int id)
        {
            return Ok(_customersService.GetPartnerships(id));
        }

        [HttpPost]
        public IHttpActionResult SavePartnership(PartnershipDto model)
        {
            _customersService.SavePartnership(model);

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult DeletePartnership(PartnershipDto model)
        {
            _customersService.DeletePartnership(model);

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult SaveVendor(VendorDto model)
        {
            _customersService.SaveCustomerVendor(model);

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult DeleteVendor(VendorDto model)
        {
            _customersService.DeleteCustomerVendor(model);

            return Ok();
        }

        [HttpGet]
        public IHttpActionResult GetPartners()
        {
            return Ok(_customersService.GetPartners());
        }

        [HttpGet]
        public IHttpActionResult GetVendors()
        {
            return Ok(_customersService.GetVendors());
        }

        [HttpGet]
        public IHttpActionResult GetVendorTypes()
        {
            return Ok(_customersService.GetVendorTypes());
        }

        [HttpGet]
        public IHttpActionResult GetProjectDescriptions()
        {
            return Ok(_customersService.GetProjectDescriptions());
        }

        [HttpGet]
        public IHttpActionResult AutocompleteWithStatus(int page_limit, int page, string q = null)
        {
            return Autocomplete(page_limit, page, q, true);
        }

        [HttpGet]
        public IHttpActionResult Autocomplete(int page_limit, int page, string q = null, bool includeStatus = false)
        {
            if (!string.IsNullOrEmpty(q) && q.StartsWith("*"))
            {
                return Ok(new[] { new { Id = (long)-1, Value = "* ALL CUSTOMERS" } });
            }

            var hasQ = !string.IsNullOrEmpty(q);
            var recentIds = new List<long>();
            List<CompanyAutocompleteDto> data;

            if (page == 1 && !hasQ)
            {
                data = _cache.Check("RECENT_CUSTOMERS") as List<CompanyAutocompleteDto>;
                if (data == null)
                {
                    recentIds = _customerAuditService.GetRecentCustomers(_membership.UserId);
                    recentIds.Reverse();

                    data = _customersService.GetAllAutocomplete()
                                            .Where(x => recentIds.Contains(x.Id))
                                            .ToList();

                    foreach (var id in recentIds)
                    {
                        var item = data.FirstOrDefault(x => x.Id == id);
                        if (item != null)
                        {
                            data.Remove(item);
                            data.Insert(0, item);
                        }
                    }

                    _cache.Put("RECENT_CUSTOMERS", data);
                }
            }
            else
            {
                IQueryable<CompanyAutocompleteDto> querySource;
                if (hasQ)
                {
                    querySource = _customersService.GetAllAutocomplete();
                }
                else
                {
                    recentIds = _customerAuditService.GetRecentCustomers(_membership.UserId);
                    querySource = _customersService.GetAllAutocomplete() /*.Where(x => !recentIds.Contains(x.Id))*/;
                }

                if (hasQ)
                {
                    q = q.ToLower().Trim();
                    querySource =
                        querySource.Where(
                            x =>
                            x.Name.ToLower().Contains(q) || x.Code.ToLower().Contains(q) ||
                            x.Alias.ToLower().Contains(q));
                }

                var query = querySource.OrderBy(x => x.Name + x.Alias + x.Code);

                if (!recentIds.Any())
                {
                    data = query
                        .Skip((page - 1) * page_limit)
                        .Take(page_limit)
                        .ToList();
                }
                else
                {
                    data = query
                        .Skip((page - 2) * page_limit + recentIds.Count())
                        .Take(page_limit)
                        .ToList();
                }
            }

            return Ok(data.Select(x => MapToAutocompleteResult(x, includeStatus)));
        }

        [HttpGet]
        [RequireAuthTokenApi]
        public IHttpActionResult AutocompleteSimple(int page_limit, int page, string q = null)
        {
            var query = _customersService.GetAllAutocomplete();

            if (string.IsNullOrEmpty(q))
            {
                query = query.OrderBy(x => x.Name + x.Alias + x.Code);
            }
            else
            {
                q = q.ToLower().Trim();

                query = query.Where(x =>
                                    (x.Name != null && x.Name.ToLower().Contains(q))
                                    || (x.Code != null && x.Code.ToLower().Contains(q))
                                    || (x.Alias != null && x.Alias.ToLower().Contains(q))
                                    || (x.MainContactUser != null && x.MainContactUser.Email.ToLower().Contains(q))
                                    ||
                                    (x.MainContactUser != null &&
                                     x.MainContactUser.CellPhoneNumber.ToLower().Contains(q))
                    )
                             .OrderBy(x => x.Name + x.Alias + x.Code);
            }

            var data = query
                .Skip((page - 1) * page_limit)
                .Take(page_limit)
                .ToList();

            //return Ok(data.Select(x => MapToAutocompleteResult(x)));
            return Ok(data);
        }

        private object MapToAutocompleteResult(CompanyAutocompleteDto x, bool includeStatus = false)
        {
            if (!includeStatus)
            {
                return new
                {
                    x.Id,
                    Value =
                               string.Format("{0} {1} - {2}", x.Name,
                                             string.IsNullOrEmpty(x.Alias) ? string.Empty : " - " + x.Alias, x.Code)
                };
            }
            return new
            {
                x.Id,
                Value =
                           string.Format("{0} {1} - {2}", x.Name,
                                         string.IsNullOrEmpty(x.Alias) ? string.Empty : " - " + x.Alias, x.Code),
                x.Status,
                x.StatusLastUpdated,
                IsStatusExpired = x.StatusValidDate < DateTime.UtcNow
            };
        }

        [HttpPost]
        public HttpResponseMessage Datatables(
            [ModelBinder(typeof(DataTableModelBinderProvider))] CustomersDataTableRequest requestModel)
        {
            var query = _customersService.GetAllCustomers().Where(c => !c.IsDeleted);

            if (requestModel.IsShowDeleted)
            {
                query = _customersService.GetAllCustomers().Where(c => c.IsDeleted);
            }

            if (!string.IsNullOrEmpty(requestModel.Name))
            {
                query = query.Where(x =>
                                    (!string.IsNullOrEmpty(x.Code) &&
                                     x.Code.ToLower().Contains(requestModel.Name.ToLower()))
                                    ||
                                    (!string.IsNullOrEmpty(x.Name) &&
                                     x.Name.ToLower().Contains(requestModel.Name.ToLower())));
            }

            if (!string.IsNullOrEmpty(requestModel.Address))
            {
                query =
                    query.Where(
                        x =>
                        !string.IsNullOrEmpty(x.Address) && x.Address.ToLower().Contains(requestModel.Address.ToLower()));
            }

            var result = _datatableService.GetResponse(_mappingService.Project<Company, CompanyShortDto>(query), requestModel);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        public IHttpActionResult GetUsers(long id)
        {
            var users = _userService.QueryUsers(false)
                                    .Where(
                                        x =>
                                        x.ClientCompanyId.HasValue && x.ClientCompanyId.Value == id &&
                                        x.IsMainContactUser);

            return Ok(users);
        }

        #region Data tables adapter

        protected override IQueryable<CompanyShortDto> GetFilteredEntities()
        {
            return GetFilteredEntities(null);
        }

        protected override IQueryable<CompanyShortDto> GetFilteredEntities(dynamic customFilter)
        {
            var query = _customersService.GetAllCustomers().Where(c => !c.IsDeleted);

            var customFilterDictionary = customFilter as Dictionary<string, object>;

            if (customFilterDictionary != null)
            {
                if (customFilterDictionary.ContainsKey("showDeleted") && customFilterDictionary["showDeleted"] != null)
                {
                    bool showDeleted;
                    if (bool.TryParse(customFilterDictionary["showDeleted"].ToString(), out showDeleted) && showDeleted)
                    {
                        query = _customersService.GetAllCustomers();
                    }
                }

                if (customFilterDictionary.ContainsKey("q") && customFilterDictionary["q"] != null)
                {
                    var q = customFilterDictionary["q"].ToString();
                    if (!string.IsNullOrEmpty(q))
                    {
                        query = query.Where(x => x.Code.Contains(q) || x.Name.Contains(q));
                    }
                }
                if (customFilterDictionary.ContainsKey("address") && customFilterDictionary["address"] != null)
                {
                    var address = customFilterDictionary["address"].ToString();
                    if (!string.IsNullOrEmpty(address))
                    {
                        query = query.Where(x => x.Address.ToLower().Contains(address.ToLower()));
                    }
                }
                /*                if (customFilterDictionary.ContainsKey("contactPhone") && customFilterDictionary["contactPhone"] != null)
                                {
                                    var contactPhone = customFilterDictionary["contactPhone"].ToString();
                                    if (!string.IsNullOrEmpty(contactPhone))
                                    {
                                        contactPhone = contactPhone.Replace("-", String.Empty);
                                        query = query.Where(x => x.CustomerContacts.Any(y => y.PhoneNumber.Replace("-", String.Empty).Contains(contactPhone)));
                                    }
                                }
                                if (customFilterDictionary.ContainsKey("contactEmail") && customFilterDictionary["contactEmail"] != null)
                                {
                                    var contactEmail = customFilterDictionary["contactEmail"].ToString();
                                    if (!string.IsNullOrEmpty(contactEmail))
                                    {
                                        query = query.Where(x => x.CustomerContacts.Any(y => y.Email.Contains(contactEmail)));
                                    }
                                }*/
            }

            return _mappingService.Project<Company, CompanyShortDto>(query);
        }

        protected override void TableCustomize(DataTablesConfig<CompanyShortDto> dtCfg)
        {
            dtCfg.Set.Properties(x => x.Id,
                                 x => x.Name,
                                 x => x.Code,
                                 x => x.Alias,
                                 x => x.Address,
                                 x => x.Status,
                                 x => x.IsDeleted);
        }

        #endregion Data tables adapter
    }
}