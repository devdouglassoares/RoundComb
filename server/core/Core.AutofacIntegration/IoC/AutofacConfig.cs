using Autofac;
using Autofac.Extras.CommonServiceLocator;
//using Autofac.Integration.Mvc;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;
using Core.IoC;
using Core.WebApi;
using Microsoft.AspNet.SignalR;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Mvc;
using AutofacDependencyResolver = Autofac.Integration.SignalR.AutofacDependencyResolver;

namespace Core.AutofacIntegration.IoC
{
    public class AutofacServiceResolver : AutofacServiceLocator, IServiceResolver
    {
        public AutofacServiceResolver(IComponentContext container) : base(container)
        {
        }
    }

    public class AutofacConfig : IContainerConfig
    {
        public void ConfigureContainer()
        {
            FakeAutofacContainer container;
            ConfigureContainer(out container);
        }

        public void ConfigureContainer<T>(out T containerObject) where T : class
        {
            var containerBuilder = new ContainerBuilder();

            var assemblies = ApplicationContainer.ApplicationAssemblies.ToArray();

            containerBuilder.RegisterAssemblyModules(assemblies);

            containerBuilder.RegisterHubs(assemblies);

            var definedConcreteTypes = assemblies.GetDefinedConcreteTypes();

            var dependencyTypes = definedConcreteTypes
                .Where(
                    type =>
                        type.GetInterfaces()
                            .Any(intf => intf.Name.Equals("IDependency") || intf.Name.Equals("ISingletonDependency")));

            InstallDependencies(dependencyTypes, containerBuilder);

            var selfRegisterType = definedConcreteTypes
                .Where(type => !type.IsGenericType)
                .Where(
                    type =>
                        type.GetInterfaces()
                            .Any(
                                intf =>
                                    intf.Name.Equals("ISingletonSelfRegisterDependency") ||
                                    intf.Name.Equals("ISelfRegisterDependency")));

            InstallSelfRegisterTypes(selfRegisterType, containerBuilder);
            containerBuilder.RegisterApiControllers(assemblies);
            //containerBuilder.RegisterControllers(assemblies);

            var container = containerBuilder.Build();

            var autofacCommonServiceLocator = new AutofacServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => autofacCommonServiceLocator);

            if (GlobalConfiguration.Configuration != null)
            {
                GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
                GlobalConfiguration.Configuration.Services.Add(typeof(IExceptionLogger), new CustomizedExceptionLogger());
            }

            DependencyResolver.SetResolver(autofacCommonServiceLocator);
            GlobalHost.DependencyResolver = new AutofacDependencyResolver(container);

            containerObject = container as T;
        }

        private void InstallSelfRegisterTypes(IEnumerable<Type> dependencyTypes, ContainerBuilder container)
        {
            foreach (var dependencyType in dependencyTypes)
            {
                var interfaces = dependencyType.GetInterfaces();

                var componentRegistration = container.RegisterType(dependencyType)
                                                     .AsSelf();

                componentRegistration =
                    interfaces.Any(x => x.Name.Equals("ISingletonSelfRegisterDependency"))
                        ? componentRegistration.SingleInstance()
                        : componentRegistration.InstancePerLifetimeScope();
            }
        }

        private void InstallDependencies(IEnumerable<Type> dependencyTypes, ContainerBuilder container)
        {
            foreach (var dependencyType in dependencyTypes)
            {
                var interfaces = dependencyType.GetInterfaces()
                                               .Where(
                                                   x =>
                                                       !x.Name.Equals("IDependency") &&
                                                       !x.Name.Equals("ISingletonDependency"))
                                               .ToArray();
                interfaces = interfaces.Except(interfaces.SelectMany(t => t.GetInterfaces())).ToArray();

                foreach (var intf in interfaces)
                {
                    if (intf.IsGenericType && dependencyType.IsGenericType)
                    {
                        var componentRegistration =
                            container.RegisterGeneric(dependencyType.GetGenericTypeDefinition())
                                     .As(intf.GetGenericTypeDefinition());

                        componentRegistration = interfaces.Any(x => x.Name.Equals("ISingletonDependency"))
                            ? componentRegistration.SingleInstance()
                            : componentRegistration.InstancePerLifetimeScope();
                    }
                    else
                    {
                        var componentRegistration = container.RegisterType(dependencyType).As(intf);
                        componentRegistration = interfaces.Any(x => x.Name.Equals("ISingletonDependency"))
                            ? componentRegistration.SingleInstance()
                            : componentRegistration.InstancePerLifetimeScope();
                    }
                }
            }
        }

        private class FakeAutofacContainer
        {
        }
    }
}