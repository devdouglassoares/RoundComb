using Core.WebApi;
using Core.WebApi.Cors;
using Core.WebApi.RouteProviders;
using System.Web.Http;

namespace CustomForm.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            CorsPolicyProviderFactory.EnableCors(config);

            // Web API configuration and services
            config.AcceptJsonFormatterOnly();

            // Web API routes
            config.MapHttpAttributeRoutes(new CentralizedPrefixProvider(new []
                                                                        {
                                                                            typeof(WebApiConfig).Assembly.GetName().Name
                                                                        }));

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints: new { id = @"^(\s*|\d+)$" }
            );

            config.Routes.MapHttpRoute(
                name: "ApiAction",
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
