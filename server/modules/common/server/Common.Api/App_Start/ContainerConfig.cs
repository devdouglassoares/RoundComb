using Castle.Windsor;
using Core.CastleWindsorIntegration.IoC;
using Core.IoC;

namespace Common.Api.App_Start
{
    public class ContainerConfig
    {
        public static IWindsorContainer Register()
        {

            ApplicationContainer.CreateApplicationContainer(new CastleWindsorConfig());

            return null;
        }
    }
}