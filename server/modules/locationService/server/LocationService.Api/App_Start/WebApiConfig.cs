using Core.WebApi;
using Core.WebApi.Cors;
using Core.WebApi.RouteProviders;
using System.Web.Http;

namespace LocationService.Api
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

            CorsPolicyProviderFactory.EnableCors(config);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
