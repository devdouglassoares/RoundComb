using Membership.Core.Contracts.AuthAttributes;
using Roundcomb.Core.Services;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Roundcomb.Core.Dtos;

namespace Roundcomb.Api.Controllers
{
    [RoutePrefix("api/property")]
    public class PropertyFormConfigController : ApiController
    {
        private readonly IPropertyFormConfigService _propertyFormConfigService;

        public PropertyFormConfigController(IPropertyFormConfigService propertyFormConfigService)
        {
            _propertyFormConfigService = propertyFormConfigService;
        }

        [HttpGet, Route("{propertyId:int}/formConfig")]
        [RequireAuthTokenApi]
        public HttpResponseMessage GetPropertyFormConfig(long propertyId)
        {
            var formConfig = _propertyFormConfigService.GetFormConfigurationSettingForProperty(propertyId);

            return Request.CreateResponse(HttpStatusCode.OK, formConfig);
        }

        [HttpPost, Route("{propertyId:int}/formConfig")]
        [RequireAuthTokenApi]
        public HttpResponseMessage SetPropertyFormConfig(long propertyId, PropertyApplicationFormDocumentConfigDto model)
        {
            _propertyFormConfigService.SaveFormConfigurationSettingForProperty(propertyId, model);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}