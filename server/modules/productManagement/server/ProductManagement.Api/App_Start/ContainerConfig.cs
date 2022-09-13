using Core.CastleWindsorIntegration.IoC;
using Core.IoC;

namespace ProductManagement.Api
{
    public class ContainerConfig
    {
        public static void Register()
        {
            ApplicationContainer.CreateApplicationContainer(new CastleWindsorConfig());
        }
    }
}