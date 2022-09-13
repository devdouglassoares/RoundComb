using Core.CastleWindsorIntegration;
using Microsoft.Owin;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

[assembly: OwinStartup(typeof(SignalRStartup))]

namespace PaymentGateway.Stripe.Api
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