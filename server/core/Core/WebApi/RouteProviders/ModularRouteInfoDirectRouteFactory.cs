using Core.Logging;
using System;
using System.Diagnostics.Contracts;
using System.Web.Http;
using System.Web.Http.Routing;

namespace Core.WebApi.RouteProviders
{
    public class ModularRouteInfoDirectRouteFactory : IDirectRouteFactory
    {
        private readonly RouteAttribute _infoProvider;
        private readonly string _modulePrefix;
        private readonly ILogger _logger = Logger.GetLogger<ModularRouteInfoDirectRouteFactory>();

        public ModularRouteInfoDirectRouteFactory(RouteAttribute infoProvider, string modulePrefix)
        {
            if (infoProvider == null)
            {
                throw new ArgumentNullException("infoProvider");
            }

            _infoProvider = infoProvider;
            _modulePrefix = modulePrefix;
        }

        public RouteEntry CreateRoute(DirectRouteFactoryContext context)
        {
            Contract.Assert(context != null);

            var routeTemplate = _infoProvider.Template.Replace("~", "~/" + _modulePrefix).Trim('/');

            IDirectRouteBuilder builder;
            try
            {
                builder = context.CreateBuilder(routeTemplate);
            }
            catch (Exception exception)
            {
                _logger.Error("Error building route template: " + routeTemplate, exception);
                throw;
            }
            Contract.Assert(builder != null);

            builder.Name = _infoProvider.Name;

            return builder.Build();
        }
    }
}