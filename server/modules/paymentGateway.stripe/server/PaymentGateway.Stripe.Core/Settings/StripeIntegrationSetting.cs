using Core.SiteSettings;

namespace PaymentGateway.Stripe.Core.Settings
{
    public class StripeIntegrationSetting : ISiteSettingBase
    {
        public string SecretKey { get; set; }

        public string PublishableKey { get; set; }

        public bool TestMode { get; set; }

        public string TestSecretKey { get; set; }

        public string TestPublishableKey { get; set; }

        public string GetPublishableKey()
        {
            return TestMode ? TestPublishableKey : PublishableKey;
        }

        public string GetSecretKey()
        {
            return TestMode ? TestSecretKey : SecretKey;
        }
    }
}