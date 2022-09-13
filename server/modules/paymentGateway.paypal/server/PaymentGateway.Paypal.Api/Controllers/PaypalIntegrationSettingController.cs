using Core.SiteSettings;
using PaymentGateway.Paypal.Core.Settings;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PaymentGateway.Paypal.Api.Controllers
{
    [RoutePrefix("api/paypalSetting")]
    public class PaypalIntegrationSettingController : BaseSiteSettingController<PaypalIntegrationSetting>
    {
        public PaypalIntegrationSettingController(ISiteSettingService siteSettingService) : base(siteSettingService)
        {
        }


        [HttpGet, Route("publicSettings")]
        public HttpResponseMessage GetPaypalSetting()
        {
            var paypalSetting = SiteSettingService.GetSetting<PaypalIntegrationSetting>();

            return Request.CreateResponse(HttpStatusCode.OK,
                                          new
                                          {
                                              MerchantId = paypalSetting.GetMerchantUserName(),
                                              SubmitUrl = paypalSetting.GetSubmitUrl()
                                          });
        }
    }
}