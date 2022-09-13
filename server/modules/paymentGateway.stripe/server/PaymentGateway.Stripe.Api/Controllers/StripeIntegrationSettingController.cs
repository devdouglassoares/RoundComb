using Core.SiteSettings;
using PaymentGateway.Stripe.Core.Settings;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PaymentGateway.Stripe.Api.Controllers
{
    [RoutePrefix("api/stripeSetting")]
    public class StripeIntegrationSettingController : BaseSiteSettingController<StripeIntegrationSetting>
    {
        public StripeIntegrationSettingController(ISiteSettingService siteSettingService) : base(siteSettingService)
        {
        }

        [Route("publicSettings")]
        public HttpResponseMessage GetStripeSetting()
        {
            var stripeSetting = SiteSettingService.GetSetting<StripeIntegrationSetting>();

            return Request.CreateResponse(HttpStatusCode.OK,
                                          new
                                          {
                                              stripePublishableKey = stripeSetting.GetPublishableKey()
                                          });
        }
    }
}
