using Core.WebApi;
using Core.WebApi.RouteProviders;
using System.Web.Http;

namespace PaymentGateway.Stripe.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.AcceptJsonFormatterOnly();

            // Web API routes
            config.MapHttpAttributeRoutes(new CentralizedPrefixProvider(new[]
                                                                        {
                                                                            typeof(WebApiConfig).Assembly.GetName().Name
                                                                        }));
        }
    }
}
