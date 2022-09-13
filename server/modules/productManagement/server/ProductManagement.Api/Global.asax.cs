using Core.CastleWindsorIntegration;
using Core.Logging;
using Microsoft.Owin;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

[assembly: OwinStartup(typeof(SignalRStartup))]

namespace ProductManagement.Api
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            ContainerConfig.Register();
        }

        protected void Application_End()
        {
            var logger = Logger.GetLogger<WebApiApplication>();
            logger.Info("Application is stopped");
        }
    }
}