using Core.WebApi.Cors;
using System.Web.Http;
using Core.CastleWindsorIntegration.IoC;
using Core.IoC;

namespace Upload.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            CorsPolicyProviderFactory.EnableCors(config);

            ApplicationContainer.CreateApplicationContainer(new CastleWindsorConfig());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
