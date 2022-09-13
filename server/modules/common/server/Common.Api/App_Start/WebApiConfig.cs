using Core.WebApi;
using Core.WebApi.Cors;
using Core.WebApi.RouteProviders;
using System.Web.Http;

namespace Common.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            CorsPolicyProviderFactory.EnableCors(config);

            config.AcceptJsonFormatterOnly();

            config.Routes.MapHttpRoute(
                name: "ApiDatasource",
                routeTemplate: "datasource/{controller}",
                defaults: new { action = "DataSource" }
            );

            // Web API routes
            config.MapHttpAttributeRoutes(new CentralizedPrefixProvider(new[] { "Common.Api" }));

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints: new { id = @"^(\s*|\d+)$" }
                //constraints: new { action = new NotEqualConstraint("DataSource") }
            );

            config.Routes.MapHttpRoute(
                name: "ApiAction",
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
