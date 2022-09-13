using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace Core.WebApi.RouteProviders
{
	public class CentralizedPrefixProvider : DefaultDirectRouteProvider
	{
		private readonly string[] _ignoreAssemblyNames;

		public CentralizedPrefixProvider(): this(new string[] { })
		{
		}

		public CentralizedPrefixProvider(string[] ignoreAssemblyNames)
		{
			_ignoreAssemblyNames = ignoreAssemblyNames;
		}

		protected override IReadOnlyList<IDirectRouteFactory> GetActionRouteFactories(HttpActionDescriptor actionDescriptor)
		{
			var actionRouteFactories = actionDescriptor.GetCustomAttributes<IDirectRouteFactory>(inherit: true);

			Collection<IDirectRouteFactory> originalFactories = new Collection<IDirectRouteFactory>();
			Collection<IDirectRouteFactory> overridingFactories = new Collection<IDirectRouteFactory>();

			var modulePrefix = GetModulePrefix(actionDescriptor.ControllerDescriptor);

			foreach (var actionRouteFactory in actionRouteFactories)
			{
				if (actionRouteFactory is RouteAttribute)
				{
					var template = ((RouteAttribute)actionRouteFactory).Template;
					if (template.StartsWith("~") && !string.IsNullOrEmpty(modulePrefix))
					{

						overridingFactories.Add(
												new ModularRouteInfoDirectRouteFactory(
													(RouteAttribute)actionRouteFactory,
													modulePrefix));
					}
					else
					{
						originalFactories.Add(actionRouteFactory);
					}
				}
			}

			var directRouteFactories = originalFactories.Concat(overridingFactories).ToList();

			return directRouteFactories;
		}

		private string GetModulePrefix(HttpControllerDescriptor controllerDescriptor)
		{
			var controllerAssembly = controllerDescriptor.ControllerType.Assembly.GetName().Name;


			return _ignoreAssemblyNames.Contains(controllerAssembly) ? "" : controllerAssembly;
		}

		protected override string GetRoutePrefix(HttpControllerDescriptor controllerDescriptor)
		{
			var existingPrefix = base.GetRoutePrefix(controllerDescriptor);
			var controllerAssembly = controllerDescriptor.ControllerType.Assembly.GetName().Name;

			if (existingPrefix == null)
				return _ignoreAssemblyNames.Contains(controllerAssembly) ? "" : controllerAssembly;



			return _ignoreAssemblyNames.Contains(controllerAssembly)
					   ? existingPrefix
					   : $"{controllerAssembly}/{existingPrefix}";
		}
	}
}