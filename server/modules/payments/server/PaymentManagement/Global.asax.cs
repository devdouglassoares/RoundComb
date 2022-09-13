using System.Web.Http;
using Core.CastleWindsorIntegration;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(SignalRStartup))]
namespace PaymentManagement
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            ContainerConfig.Register();
        }
    }
}
