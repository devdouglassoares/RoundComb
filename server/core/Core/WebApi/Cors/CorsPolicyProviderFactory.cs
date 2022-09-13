using Core.WebApi.ActionFilters;
using System.Net.Http;
using System.Web.Cors;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Core.WebApi.Cors
{
    public class CorsPolicyProviderFactory : ICorsPolicyProviderFactory
    {
        public ICorsPolicyProvider GetCorsPolicyProvider(HttpRequestMessage request)
        {
            var policy = new CorsPolicy
            {
                AllowAnyOrigin = true,
                AllowAnyHeader = true,
                AllowAnyMethod = true,
                SupportsCredentials = true
            };

            return new CorsPolicyProvider(policy);
        }

        public static void EnableCors(HttpConfiguration config)
        {
            config.SetCorsPolicyProviderFactory(new CorsPolicyProviderFactory());
            config.EnableCors();
            config.Filters.Add(new KeepSiteAliveAttribute());
        }
    }
}