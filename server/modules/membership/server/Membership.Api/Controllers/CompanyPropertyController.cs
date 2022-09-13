using Core.DynamicProperties.Controllers;
using Membership.Core.Entities;
using Membership.Library.Services;
using System.Web.Http;

namespace Membership.Api.Controllers
{
    [RoutePrefix("api/companyProperty")]
    public class CompanyPropertyController : DynamicPropertiesController<Company, ICompanyDynamicPropertyService>
    {
        public CompanyPropertyController(ICompanyDynamicPropertyService dynamicPropService) : base(dynamicPropService)
        {
        }
    }
}
