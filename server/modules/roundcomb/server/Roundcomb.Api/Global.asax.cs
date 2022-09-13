using System.Web.Http;

namespace Roundcomb.Api
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
