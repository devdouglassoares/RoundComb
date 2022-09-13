using Core.SiteSettings;
using LocationService.RestClient.SiteSettings;
using System.Web.Http;

namespace LocationService.RestClient.Controllers
{
    [RoutePrefix("api/locationServiceSetting")]
    public class LocationSettingController: BaseSiteSettingController<LocationServiceIntegrationSetting>
    {
        public LocationSettingController(ISiteSettingService siteSettingService) : base(siteSettingService)
        {
        }
    }
}