using Core.SiteSettings;
using Membership.FacebookOAuthIntegration.Settings;
using System.Web.Http;

namespace Membership.FacebookOAuthIntegration.Controllers
{
    [RoutePrefix("api/facebookOauthSetting")]
    public class FacebookSettingController : BaseSiteSettingController<FacebookOAuthSetting>
    {
        public FacebookSettingController(ISiteSettingService siteSettingService) : base(siteSettingService)
        {
        }
    }
}