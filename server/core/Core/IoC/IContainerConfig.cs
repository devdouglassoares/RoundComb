using Microsoft.Practices.ServiceLocation;

namespace Core.IoC
{
    public interface IContainerConfig
    {
        void ConfigureContainer<T>(out T containerObject) where T : class;
        void ConfigureContainer();
    }

    public interface IServiceResolver : IServiceLocator
    {
    }
}