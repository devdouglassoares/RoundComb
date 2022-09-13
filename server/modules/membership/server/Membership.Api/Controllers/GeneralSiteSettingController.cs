using System.Net.Http;
using System.Web.Http;
using Core.SiteSettings;
using Membership.Core.Contracts.AuthAttributes;

namespace Membership.Api.Controllers
{
    [RoutePrefix("api/GeneralSiteSetting")]
    public class GeneralSiteSettingController : BaseSiteSettingController<GeneralSiteSetting>
    {
        public GeneralSiteSettingController(ISiteSettingService siteSettingService) : base(siteSettingService)
        {
        }
    }
}