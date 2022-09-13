using Membership.Core.Contracts.AuthAttributes;
using Roundcomb.Core.Services;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Roundcomb.Api.Controllers
{
    [RoutePrefix("api/propertyCategory")]
    public class PropertyCategoryFormConfigController : ApiController
    {
        private readonly IPropertyFormConfigService _propertyFormConfigService;

        public PropertyCategoryFormConfigController(IPropertyFormConfigService propertyFormConfigService)
        {
            _propertyFormConfigService = propertyFormConfigService;
        }

        [HttpPost, Route("{propertyCategoryId:int}/formConfig/{formConfigurationId}")]
        [RequireAuthTokenApi]
        public HttpResponseMessage AssignPropertyFormConfig(long propertyCategoryId, long formConfigurationId)
        {
            _propertyFormConfigService.AssignFormToPropertyCategory(formConfigurationId, propertyCategoryId);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpGet, Route("{propertyCategoryId:int}/formConfig")]
        [RequireAuthTokenApi]
        public HttpResponseMessage GetPropertyFormConfig(long propertyCategoryId)
        {
            var formConfig = _propertyFormConfigService.GetFormConfigurationSettingForPropertyCategory(propertyCategoryId);

            return Request.CreateResponse(HttpStatusCode.OK, formConfig);
        }
    }
}
