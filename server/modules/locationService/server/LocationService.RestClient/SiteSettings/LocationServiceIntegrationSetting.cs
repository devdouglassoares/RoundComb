using Core.SiteSettings;

namespace LocationService.RestClient.SiteSettings
{
    public class LocationServiceIntegrationSetting : ISiteSettingBase
    {
        public bool EnableLocationService { get; set; }

        public string LocationServiceServer { get; set; }

        public string LocationServiceApiKey { get; set; }

        public string LocationServiceApiSecret { get; set; }
    }
}