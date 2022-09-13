using Core.SiteSettings;
using ProductManagement.Core.Settings;
using System.Web.Http;

namespace ProductManagement.Api.Controllers
{
    [RoutePrefix("api/propertyManagementSetting")]
    public class PropertyManagementSettingController : BaseSiteSettingController<PropertyManagementSetting>
    {
        public PropertyManagementSettingController(ISiteSettingService siteSettingService) : base(siteSettingService)
        {
        }
    }
}