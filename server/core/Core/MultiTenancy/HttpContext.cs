using System.Web;

namespace Core.MultiTenancy
{
    public static class HttpContextExtensions
    {
        private const string TenantConfigurationContextKey = "CurrentTenantInfo";

        public static void SetTenantInfo(this HttpContext httpContext, TenantConfiguration tenantConfiguration)
        {
            httpContext.Items[TenantConfigurationContextKey] = tenantConfiguration;
        }

        public static TenantConfiguration GetTenantInfo(this HttpContext httpContext)
        {
            return httpContext.Items[TenantConfigurationContextKey] as TenantConfiguration;
        }

    }
}