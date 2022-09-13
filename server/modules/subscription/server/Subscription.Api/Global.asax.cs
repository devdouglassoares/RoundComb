using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Core.CastleWindsorIntegration;
using Microsoft.Owin;

[assembly: OwinStartup(typeof (SignalRStartup))]

namespace Subscription.Api
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);

            ContainerConfig.Register();
        }
    }
}