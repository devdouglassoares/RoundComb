using Core.CastleWindsorIntegration.IoC;
using Core.IoC;

namespace PaymentManagement
{
    public class ContainerConfig
    {
        public static void Register()
        {
            ApplicationContainer.CreateApplicationContainer(new CastleWindsorConfig());
        }
    }
}