using Core.Extensions;
using DTWrapper.Core;
using Membership.Api.Controllers.Base;
using Membership.Api.Extensions;
using Membership.Api.Models;
using Membership.Core;
using Membership.Core.Contracts;
using Membership.Core.Dto;
using Membership.Core.Entities;
using Membership.Core.Entities.Enums;
using Membership.Library.Contracts;
using Membership.Library.Data;
using Membership.Library.Dto;
using Membership.Library.Extentions;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Membership.Api.Controllers
{
    public class ManageCompaniesController : DTApiController<CompanyDto>
    {
        private readonly ICompanyService _companyService;
        private readonly IMembership _membership;
        private readonly IRepository _repository;
        private readonly IUserService _userService;

        public ManageCompaniesController(IRepository repository, IMembership membership, ICompanyService companyService,
                                         IUserService userService)
        {
            _repository = repository;
            _membership = membership;
            _companyService = companyService;
            _userService = userService;
        }

        protected override IQueryable<CompanyDto> GetFilteredEntities()
        {
            return _companyService.GetAllDtos().Where(c => !c.MasterCompanyId.HasValue).AsQueryable();
        }

        protected override void TableCustomize(DataTablesConfig<CompanyDto> dtCfg)
        {
            dtCfg.Set.Properties(x => x.Id, x => x.CompanyName, x => x.Code, x => x.Alias, x => x.Address);
        }

        #region Private Methods

        private object GetJiraAccount(Company company)
        {
            if (!company.HasValidJiraAccount())
            {
                return new
                       {
                           url = "",
                           key = "",
                           login = "",
                           password = ""
                       };
            }

            return new
                   {
                       url = company.GetSetting(CompanySettings.JiraUrl),
                       key = company.GetSetting(CompanySettings.JiraProjectKey),
                       login = company.GetSetting(CompanySettings.JiraLogin),
                       password = ""
                   };
        }

        #endregion Private Methods

        #region Actions

        [HttpGet]
        public async Task<HttpResponseMessage> GetAllCompanies()
        {
            var companies =
                await
                _repository.GetAll<Company>()
                           .ToDictionaryAsync(k => k.Id, v => v.Name);
            return Request.CreateResponse(HttpStatusCode.OK, companies.ToList());
        }

        [HttpPost]
        public dynamic /*async Task<HttpResponseMessage>*/ PostCompany(CompanyModel model)
        {
            //model.Owner.Roles = new List<string> { MembershipConstant.C_ROLE_CODE_COMPANYADMIN };

            if (model.Owner == null)
            {
                //throw new Exception("Company owner is not defined");

                model.Owner = new UserBaseModel
                {
                    FirstName = model.CompanyName + "First Name",
                    LastName = model.CompanyName + "Last Name",
                    Email =
                        model.CompanyName.Replace(" ", "") + StringExtensions.GenerateRandomCode(6) +
                        "@somedomain.com"
                };
            }

            // TODO: Create user and company in transaction

            var ownerModel = _userService.Register(model.Owner);

            var owner = _repository.Get<User>(ownerModel.Id);

            var bizOwnerRole =
                _repository.GetAll<Role>()
                    .FirstOrDefault(role => role.Code == MembershipConstant.C_ROLE_CODE_COMPANYADMIN);

            owner.Roles.Add(bizOwnerRole);

            var company = new Company
            {
                Domain = model.Domain,
                Alias = model.Alias,
              //  Owner = owner,
                Name = model.CompanyName,
                Code = model.Code,
                CurrentTheme = model.Theme,
                Address = model.Address
            };

            _repository.Insert(company);

            owner.Company = company;

            _repository.SaveChanges();

            //this.companyService.AssignSubDomain(company.Domain);

            return Request.CreateResponse(HttpStatusCode.OK, new {company.Id});
        }

        [HttpPut]
        public dynamic /*async Task<HttpResponseMessage>*/ PutCompany(CompanyModel model)
        {
            var company = _repository.Get<Company>(model.Id);

            if (company.Domain != model.Domain)
            {
                _companyService.AssignSubDomain(model.Domain);
            }

            company.Domain = model.Domain;
            company.Alias = model.Alias;
            company.Name = model.CompanyName;
            company.Code = model.Code;
            company.Address = model.Address;
            company.CurrentTheme = model.Theme;

            _repository.Update(company);
            _repository.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        //        [HttpGet]
        //        [Route("api/managecompanies/{companyId}/assignDropboxToken/{type}/token")]
        //        public async Task<HttpResponseMessage> AssignDropboxToken(string companyId, string type, string acc)
        //        {
        //            
        //        }

        [HttpGet]
        public async Task<HttpResponseMessage> Get(long id)
        {
            var model = new CompanyModel();
            var company = _repository.Get<Company>(id);

            model.Id = company.Id;
            model.CompanyName = company.Name;
            model.Domain = company.Domain;
            model.LogoUrl = company.LogoUrl;
            model.Code = company.Code;
            model.Theme = company.CurrentTheme;

            model.Owner = company.Owner != null ? _userService.Get(company.Owner.Id) : null;

            return Request.CreateResponse(HttpStatusCode.OK, model);
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetCompanySettings()
        {
            if (_membership.IsAccessAllowed(PortalFeatures.EditAllCompaniesSettings))
            {
                var companies = _repository.GetAll<Company>()
                    .ToList().Select(c =>
                        new
                        {
                            id  = c.Id,
                            name = c.Name,
                            companyTheme = c.CurrentTheme,
                            domain = c.Domain,
                            jiraAccount = GetJiraAccount(c)
                        });

                return Request.CreateResponse(HttpStatusCode.OK, companies.ToList());
            }

            var companyId = _membership.GetCurrentBizOwner().Id;
            var company = _repository.GetAll<Company>().Include("Computer").FirstOrDefault(c => c.Id == companyId);
            Verifier.IsNotNull(company);
            //Verifier.IsNotNull(company.Computer);

            return Request.CreateResponse(HttpStatusCode.OK,
                (new[]
                {
                    company
                }).Select(c => new
                {
                    id = c.Id,
                    name = c.Name,
                    companyTheme = c.CurrentTheme,
                    domain = c.Domain,
                    jiraAccount = GetJiraAccount(c)
                }));
        }              

        [HttpPost]
        public async Task<HttpResponseMessage> SetCompanyTheme(string theme, long? companyId)
        {
            if (!_membership.IsAccessAllowed(PortalFeatures.EditAllCompaniesSettings) || !companyId.HasValue)
            {
                companyId = _membership.GetCurrentBizOwner().Id;
            }

            var company = _repository.GetAll<Company>().FirstOrDefault(c => c.Id == companyId);

            Verifier.IsNotNull(company);
            //Verifier.IsNotNull(company.Computer);

            company.CurrentTheme = theme;
            _repository.Update(company);
            _repository.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> SetCompanyDomain(string domain, long? companyId)
        {
            if (!_membership.IsAccessAllowed(PortalFeatures.EditAllCompaniesSettings) || !companyId.HasValue)
            {
                companyId = _membership.GetCurrentBizOwner().Id;
            }

            if (domain.Contains("."))
            {
                throw new ArgumentException("Invalid characters in domain.");
            }

            var company = _repository.GetAll<Company>().FirstOrDefault(c => c.Id == companyId);

            Verifier.IsNotNull(company);
            //Verifier.IsNotNull(company.Computer);

            var existing = _repository.GetAll<Company>().FirstOrDefault(c => c.Domain == domain);
            if (existing != null && existing.Id != company.Id)
            {
                throw new ArgumentException("Specified domain already used.");
            }

            company.Domain = domain;
            _companyService.AssignSubDomain(domain);
            _repository.Update(company);
            _repository.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> SetCompanyName(string name, long? companyId)
        {
            if (!_membership.IsAccessAllowed(PortalFeatures.EditAllCompaniesSettings) || !companyId.HasValue)
            {
                companyId = _membership.GetCurrentBizOwner().Id;
            }


            var company = _repository.GetAll<Company>().FirstOrDefault(c => c.Id == companyId);

            Verifier.IsNotNull(company);
            //Verifier.IsNotNull(company.Computer);

            var existing = _repository.GetAll<Company>().FirstOrDefault(c => c.Name == name);
            if (existing != null && existing.Id != company.Id)
            {
                throw new ArgumentException("Specified name already used.");
            }

            company.Name = name;
            _repository.Update(company);
            _repository.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> SetCompanyLogo(string logoUrl, long? companyId)
        {
            if (!_membership.IsAccessAllowed(PortalFeatures.EditAllCompaniesSettings) || !companyId.HasValue)
            {
                companyId = _membership.GetCurrentBizOwner().Id;
            }


            var company = _repository.GetAll<Company>().FirstOrDefault(c => c.Id == companyId);

            Verifier.IsNotNull(company);
            //Verifier.IsNotNull(company.Computer);

            company.LogoUrl = logoUrl;
            _repository.Update(company);
            _repository.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> SetTmsAccount(TmsAccountModel model, long? companyId)
        {
            if (!_membership.IsAccessAllowed(PortalFeatures.EditAllCompaniesSettings) || !companyId.HasValue)
            {
                companyId = _membership.GetCurrentBizOwner().Id;
            }

            var company = _repository.GetAll<Company>().FirstOrDefault(c => c.Id == companyId);

            Verifier.IsNotNull(company);
            //Verifier.IsNotNull(company.Computer);

            company.SetSetting(CompanySettings.JiraUrl, model.url);
            company.SetSetting(CompanySettings.JiraProjectKey, model.key);
            company.SetSetting(CompanySettings.JiraLogin, model.login);

            if (!string.IsNullOrEmpty(model.password))
            {
                company.SetSetting(CompanySettings.JiraPassword, model.password);
            }

            _repository.Update(company);
            _repository.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        #endregion Actions
    }
}