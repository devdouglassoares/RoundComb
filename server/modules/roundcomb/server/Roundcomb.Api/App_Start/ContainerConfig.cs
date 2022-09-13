using Core.CastleWindsorIntegration.IoC;
using Core.IoC;

namespace Roundcomb.Api
{
    public class ContainerConfig
    {
        public static void Register()
        {
            ApplicationContainer.CreateApplicationContainer(new CastleWindsorConfig());
        }
    }
}