using Core.Logging;
using System;
using System.Web;
using System.Web.Http;

namespace CustomForm.Api
{
    public class Global : HttpApplication
    {
        private readonly ILogger Logger = global::Core.Logging.Logger.GetLogger<Global>();
        protected void Application_Start(object sender, EventArgs e)
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            ContainerConfig.Register();
        }


        protected virtual void Application_Error(object sender, EventArgs e)
        {
            var exc = Server.GetLastError();
            Logger.Error(exc);
        }
    }
}