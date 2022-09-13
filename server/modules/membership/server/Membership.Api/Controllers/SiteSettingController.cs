using Core.SiteSettings;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Membership.Api.Controllers
{
    [RoutePrefix("api/sitesettings")]
    public class SiteSettingController : ApiController
    {
        private readonly ISiteSettingService _siteSettingService;

        public SiteSettingController(ISiteSettingService siteSettingService)
        {
            _siteSettingService = siteSettingService;
        }

        [HttpGet, Route("availableSettings")]
        public HttpResponseMessage GetAllAvailableSettings()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _siteSettingService.GetAllAvailableSettings());
        }
    }
}