using Core.SiteSettings;

namespace Membership.FacebookOAuthIntegration.Settings
{
    public class FacebookOAuthSetting : ISiteSettingBase
    {
        public string AppId { get; set; }

        public string AppSecret { get; set; }
    }
}