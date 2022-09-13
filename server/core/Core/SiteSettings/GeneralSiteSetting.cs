using System.Collections.Generic;

namespace Core.SiteSettings
{
    public class GeneralSiteSetting : ISiteSettingBase
    {
        public string SiteName { get; set; }

        public string SiteCulture { get; set; }

        public string SiteTheme { get; set; }

        public string SiteCountry { get; set; }

        public string AdminContactEmail { get; set; }

        public List<AdministratorContactEmailSetting> AdminContactEmails { get; set; }

        public string DateTimeFormat { get; set; }
    }

    public class AdministratorContactEmailSetting
    {
        public string EmailAddress { get; set; }

        public string DisplayName { get; set; }

        public bool IsDefaultToSendNotification { get; set; }
    }
}