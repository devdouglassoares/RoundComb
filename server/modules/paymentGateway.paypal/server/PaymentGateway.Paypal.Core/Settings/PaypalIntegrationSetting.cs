using Core.SiteSettings;

namespace PaymentGateway.Paypal.Core.Settings
{
    public class PaypalIntegrationSetting : ISiteSettingBase
    {
        public PaypalIntegrationSetting()
        {
            // load default endpoint
            SubmitEndpointUrl = "https://www.paypal.com/cgi-bin/webscr";
            TestSubmitEndpointUrl = "https://www.sandbox.paypal.com/cgi-bin/webscr";
        }

        public string MerchantUserName { get; set; }

        public string SubmitEndpointUrl { get; set; }

        public bool TestMode { get; set; }

        public string TestMerchantUserName { get; set; }

        public string TestSubmitEndpointUrl { get; set; }

        public string GetMerchantUserName()
        {
            return TestMode ? TestMerchantUserName : MerchantUserName;
        }

        public string GetSubmitUrl()
        {
            return TestMode ? TestSubmitEndpointUrl : SubmitEndpointUrl;
        }
    }
}