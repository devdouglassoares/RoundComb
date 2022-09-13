using Membership.Core;
using Membership.Core.Contracts.AuthAttributes;
using Membership.Library.Contracts;
using Membership.Library.Dto;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Membership.Api.Controllers
{
    [RoutePrefix("api/company")]
    public class CompanyController : ApiController
    {
        private readonly ICompanyService _companyService;
        private readonly IMembership _memberhip;

        public CompanyController(ICompanyService companyService, IMembership memberhip)
        {
            _companyService = companyService;
            _memberhip = memberhip;
        }

        [HttpPost, RequireAuthTokenApi]
        [Route("myCompany")]
        public HttpResponseMessage CreateCompanyForCurrentLogInUser(CompanyDto companyModel)
        {
            var ownerId = _memberhip.UserId;

            _companyService.CreateCompanyForUser(companyModel, ownerId);
            return Request.CreateResponse(HttpStatusCode.Created);
        }
    }
}
