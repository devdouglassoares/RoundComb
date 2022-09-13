using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Core.IoC;
using Core.Logging;
using Core.WebApi;
using Microsoft.AspNet.SignalR;
using Microsoft.Practices.ServiceLocation;
using SignalR.Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using System.Web.Mvc;

namespace Core.CastleWindsorIntegration.IoC
{
    public class CastleWindsorServiceResolver : WindsorServiceLocator, IServiceResolver
    {
        public CastleWindsorServiceResolver(IWindsorContainer container) : base(container)
        {
        }
    }

    public class CastleWindsorConfig : IContainerConfig
    {
        private class FakeWindsorCastleContainer : WindsorContainer { }

        private readonly ILogger _logger = Logger.GetLogger<CastleWindsorConfig>();

        public void ConfigureContainer<T>(out T containerObject) where T : class
        {
            var container = new WindsorContainer();
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));

            container.Register(Component.For<IWindsorContainer>().UsingFactoryMethod(() => container));
            container.Register(Component.For<WindsorContainer>().UsingFactoryMethod(() => container));
            container.Register(Component.For<ILazyComponentLoader>().ImplementedBy<LazyOfTComponentLoader>());

            var assemblies = ApplicationContainer.ApplicationAssemblies.ToArray();

            var definedConcreteTypes = assemblies.GetDefinedConcreteTypes();

            var installerClasses =
                definedConcreteTypes
                          .Where(type => typeof(IWindsorInstaller).IsAssignableFrom(type));

            InstallInstallerClasses(installerClasses, container);

            var dependencyTypes = definedConcreteTypes
                                            .Where(type => type.GetInterfaces().Any(intf => intf.Name.Equals("IDependency") || intf.Name.Equals("ISingletonDependency")));

            InstallDependencies(dependencyTypes, container);

            var selfRegisterType = definedConcreteTypes
                                            .Where(type => !type.IsGenericType)
                                            .Where(type => type.GetInterfaces().Any(intf => intf.Name.Equals("ISingletonSelfRegisterDependency") || intf.Name.Equals("ISelfRegisterDependency")));

            InstallSelfRegisterTypes(selfRegisterType, container);

            var controllerTypes = definedConcreteTypes
                                            .Where(type => !type.IsGenericType)
                                            .Where(type => typeof(Controller).IsAssignableFrom(type));

            InstallControllerClasses(controllerTypes, container);

            var apiControllerTypes = definedConcreteTypes
                                            .Where(type => !type.IsGenericType)
                                            .Where(type => typeof(ApiController).IsAssignableFrom(type));

            InstallApiControllerClasses(apiControllerTypes, container);

            var signalRHubs = definedConcreteTypes
                                            .Where(type => !type.IsGenericType)
                                            .Where(type => typeof(Hub).IsAssignableFrom(type));

            InstallSignalRHubs(signalRHubs, container);

            var windsorServiceLocator = new CastleWindsorServiceResolver(container);
            ServiceLocator.SetLocatorProvider(() => windsorServiceLocator);

            container.Register(Component.For<IServiceResolver>().UsingFactoryMethod(() => windsorServiceLocator));

            if (GlobalConfiguration.Configuration != null)
            {
                GlobalConfiguration.Configuration.DependencyResolver = new IoCContainer(container);
                GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator),
                                                                   new WindsorHttpControllerActivator(container));
                GlobalConfiguration.Configuration.Services.Add(typeof(IExceptionLogger),
                                                               new CustomizedExceptionLogger());
            }
            DependencyResolver.SetResolver(windsorServiceLocator);
            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container));
            GlobalHost.DependencyResolver = new WindsorDependencyResolver(container);


            containerObject = container as T;
        }

        public void ConfigureContainer()
        {
            FakeWindsorCastleContainer container;
            ConfigureContainer(out container);
        }

        private void InstallControllerClasses(IEnumerable<Type> controllerTypes, IWindsorContainer container)
        {
            container.Register(Classes.From(controllerTypes).BasedOn<Controller>().LifestyleTransient());
        }

        private void InstallApiControllerClasses(IEnumerable<Type> controllerTypes, IWindsorContainer container)
        {
            container.Register(Classes.From(controllerTypes).BasedOn<ApiController>().LifestyleTransient());
        }

        private void InstallSignalRHubs(IEnumerable<Type> controllerTypes, IWindsorContainer container)
        {
            container.Register(Classes.From(controllerTypes).BasedOn<Hub>().LifestyleTransient());
        }

        private void InstallInstallerClasses(IEnumerable<Type> installerClasses, IWindsorContainer container)
        {
            foreach (var installerClass in installerClasses)
            {
                try
                {
                    var installer = Activator.CreateInstance(installerClass) as IWindsorInstaller;
                    if (installer == null) continue;

                    container.Install(installer);
                }
                catch (Exception exception)
                {
                    _logger.Fatal("Error while installing installer class", exception);
                }
            }
        }

        private void InstallSelfRegisterTypes(IEnumerable<Type> dependencyTypes, IWindsorContainer container)
        {
            foreach (var dependencyType in dependencyTypes)
            {
                var interfaces = dependencyType.GetInterfaces();

                var componentRegistration = Component.For(dependencyType).ImplementedBy(dependencyType);

                componentRegistration =
                    interfaces.Any(x => x.Name.Equals("ISingletonSelfRegisterDependency"))
                        ? componentRegistration.LifestyleSingleton()
                        : componentRegistration.LifeStyle.HybridPerWebRequestPerThread();


                try
                {
                    container.Register(componentRegistration);
                }
                catch (Exception exception)
                {
                    _logger.Fatal($"Error while register class {dependencyType}", exception);
                }
            }
        }
        private void InstallDependencies(IEnumerable<Type> dependencyTypes, IWindsorContainer container)
        {
            foreach (var dependencyType in dependencyTypes)
            {
                var interfaces = dependencyType.GetInterfaces()
                                               .Where(
                                                   x =>
                                                       !x.Name.Equals("IDependency") &&
                                                       !x.Name.Equals("ISingletonDependency"))
                                               .ToArray();

                interfaces = interfaces.Concat(interfaces.SelectMany(t => t.GetInterfaces())).Distinct().ToArray();

                if (interfaces.Length == 0)
                    continue;

                ComponentRegistration<object> componentRegistration;
                var registrationType = dependencyType.IsGenericType
                    ? dependencyType.GetGenericTypeDefinition()
                    : dependencyType;

                if (dependencyType.IsGenericType)
                {
                    componentRegistration =
                        Component.For(
                            interfaces.Select(intf => intf.IsGenericType ? intf.GetGenericTypeDefinition() : intf))
                                 .ImplementedBy(registrationType);
                }
                else
                {
                    componentRegistration =
                        Component.For(interfaces).ImplementedBy(registrationType);
                }

                componentRegistration = interfaces.Any(x => x.Name.Equals("ISingletonDependency"))
                        ? componentRegistration.LifestyleSingleton()
                        : componentRegistration.LifeStyle.HybridPerWebRequestPerThread();

                if (componentRegistration == null)
                {
                    continue;
                }

                try
                {
                    container.Register(componentRegistration);
                }
                catch (Exception exception)
                {
                    _logger.Fatal($"Error while register class {dependencyType}", exception);
                }
            }
        }
    }
}
