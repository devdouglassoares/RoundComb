using System.Net.Http;
using System.Web.Http;
using Core.SiteSettings;
using Membership.Core.Contracts.AuthAttributes;
using Membership.Core.SiteSettings.Models;

namespace Membership.Api.Controllers
{
    [RoutePrefix("api/membershipSetting")]
    public class MembershipSettingController : BaseSiteSettingController<MembershipSetting>
    {
        public MembershipSettingController(ISiteSettingService siteSettingService) : base(siteSettingService)
        {
        }
    }
}