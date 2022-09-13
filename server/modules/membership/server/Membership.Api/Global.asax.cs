using System.Web.Http;

namespace Membership.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ContainerConfig.Register();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
