using Core.CastleWindsorIntegration.IoC;
using Core.IoC;

namespace Subscription.Api
{
    public class ContainerConfig
    {
        public static void Register()
        {
            ApplicationContainer.CreateApplicationContainer(new CastleWindsorConfig());
        }
    }
}