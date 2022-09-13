using Core.MultiTenancy;
using System;
using System.Web;

namespace Core
{
    public abstract class MultiTenantWebApplication : HttpApplication
    {
        private static Startup<ApplicationHost> _startup;

        protected MultiTenantWebApplication()
        {
            AddOnBeginRequestAsync(StartupHttpModule.BeginBeginRequest, StartupHttpModule.EndBeginRequest, null);
        }

        protected virtual void Application_Start()
        {
            _startup = new Startup<ApplicationHost>(HostInitialization, HostBeginRequest, HostEndRequest, HostApplicationError);
            _startup.OnApplicationStart(this);
        }

        protected void Application_BeginRequest()
        {
            _startup.OnBeginRequest(this);
        }

        protected void Application_EndRequest()
        {
            _startup.OnEndRequest(this);
        }

        private static void HostBeginRequest(HttpApplication application, ApplicationHost host)
        {
            host.BeginRequest();
        }

        private static void HostEndRequest(HttpApplication application, ApplicationHost host)
        {
            host.EndRequest();
        }

        private static void HostApplicationError(HttpApplication application, EventArgs e, ApplicationHost host)
        {

        }

        private ApplicationHost HostInitialization(HttpApplication application)
        {
            var host = new ApplicationHost();
            
            return host;
        }

        protected virtual void Application_Error(object sender, EventArgs e)
        {
            _startup.OnApplicationError(this, e);
        }
    }
}
