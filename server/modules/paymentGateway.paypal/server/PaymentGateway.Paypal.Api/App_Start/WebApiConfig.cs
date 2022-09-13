using Core.WebApi;
using System.Web.Http;
using Core.WebApi.RouteProviders;

namespace PaymentGateway.Paypal.Api
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
