using Microsoft.AspNet.SignalR;
using Owin;

namespace Core.CastleWindsorIntegration
{
	public class SignalRStartup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR(new HubConfiguration
                           {
	                           EnableDetailedErrors = true
                           });
        }
    }
}