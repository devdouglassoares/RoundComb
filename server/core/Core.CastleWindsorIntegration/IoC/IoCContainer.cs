using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Castle.Windsor;

namespace Core.CastleWindsorIntegration.IoC
{
    public class IoCContainer : IDependencyResolver
    {
        protected IWindsorContainer Container;

        public IoCContainer(IWindsorContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            this.Container = container;
        }

        public object GetService(Type serviceType)
        {
            return Container.Kernel.HasComponent(serviceType) ? Container.Resolve(serviceType) : null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Container.Kernel.HasComponent(serviceType)
                ? Container.ResolveAll(serviceType).Cast<object>()
                : new object[0];
        }

        public void Dispose()
        {
            Container.Dispose();
        }

        public IDependencyScope BeginScope()
        {
            var child = new WindsorContainer();
            Container.AddChildContainer(child);
            return new IoCContainer(child);
        }
    }
}