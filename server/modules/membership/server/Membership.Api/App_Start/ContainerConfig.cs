using Core.CastleWindsorIntegration.IoC;
using Core.IoC;

namespace Membership.Api
{
    public static class ContainerConfig
    {
        public static void Register()
        {
            ApplicationContainer.CreateApplicationContainer(new CastleWindsorConfig());
        }
    }
}