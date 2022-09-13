using Core.WebApi;
using Core.WebApi.Cors;
using Core.WebApi.RouteProviders;
using System.Web.Http;

namespace Membership.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            CorsPolicyProviderFactory.EnableCors(config);

            // Web API routes
            config.MapHttpAttributeRoutes(new CentralizedPrefixProvider(new[] { "Membership.Api" }));

            config.Routes.MapHttpRoute(
                name: "ApiDatasource",
                routeTemplate: "datasource/{controller}",
                defaults: new { action = "DataSource" }
                );


            config.Routes.MapHttpRoute(
                name: "ApiDefault",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints: new { id = @"\d*" }
                );

            config.Routes.MapHttpRoute(
                name: "ApiAction",
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new { action = "Index", id = RouteParameter.Optional }
                );

            config.AcceptJsonFormatterOnly();
        }
    }
}
