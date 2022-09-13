#region References

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;

#endregion

namespace Core.MultiTenancy
{
    public class ApplicationHost
    {
        private static readonly object SyncLock = new object();

        private readonly IDictionary<string, TenantConfiguration> _tenantConfigurations;
        public static bool SupportMultitenant;
        public static string MultitenantServerEndpoint;

        public ApplicationHost()
        {
            _tenantConfigurations = new ConcurrentDictionary<string, TenantConfiguration>();

            bool.TryParse(WebConfigurationManager.AppSettings["MultiTenantSupport"], out SupportMultitenant);
            MultitenantServerEndpoint = WebConfigurationManager.AppSettings["MultiTenantServerEndpoint"];
        }

        public void BeginRequest()
        {
            var httpContext = HttpContext.Current;
            var request = httpContext.Request;

            if (!SupportMultitenant || string.IsNullOrEmpty(MultitenantServerEndpoint))
                return;

            lock (SyncLock)
            {
                var tenantInformation = RequestTenantInfoFromService(request, request.Headers);
                httpContext.SetTenantInfo(tenantInformation);
            }
        }

        private TenantConfiguration RequestTenantInfoFromService(HttpRequest request, NameValueCollection requestHeaders)
        {
            var origin = requestHeaders["Origin"];

            if (origin == null)
                return RequestTenantByClientIdAndSecret(request, requestHeaders);

            return RequestTenantByOrigin(origin, requestHeaders);
        }

        private TenantConfiguration RequestTenantByOrigin(string origin, NameValueCollection requestHeaders)
        {
            if (_tenantConfigurations.ContainsKey(origin) && _tenantConfigurations[origin] != null)
            {
                return _tenantConfigurations[origin];
            }

            var tenantConfiguration = RequestTenantConfig(client =>
                                                          {
                                                              client.Headers["Origin"] = requestHeaders["Origin"];
                                                          });

            if (tenantConfiguration != null)
            {
                _tenantConfigurations.Add(origin, tenantConfiguration);
            }

            return tenantConfiguration;
        }

        private TenantConfiguration RequestTenantConfig(Action<WebClient> webclientInteract)
        {
            TenantConfiguration tenantConfiguration;
            var webClient = new WebClient();

            webclientInteract(webClient);

            try
            {
                var tenantInformationJsonString = webClient.DownloadString(MultitenantServerEndpoint);
                tenantConfiguration = new JavaScriptSerializer().Deserialize<TenantConfiguration>(tenantInformationJsonString);
            }
            catch (Exception)
            {
                tenantConfiguration = null;
            }

            return tenantConfiguration;
        }

        private TenantConfiguration RequestTenantByClientIdAndSecret(HttpRequest request, NameValueCollection requestHeaders)
        {
            var clientId = requestHeaders["ClientId"];
            var clientSecret = requestHeaders["ClientSecret"];

            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
                throw new InvalidOperationException("You are not allow to access this resource");

            var key = clientId + "|||" + clientSecret;

            if (_tenantConfigurations.ContainsKey(key) && _tenantConfigurations[key] != null)
            {
                return _tenantConfigurations[key];
            }

            var tenantConfiguration = RequestTenantConfig(client =>
            {
                client.Headers["ClientId"] = requestHeaders["ClientId"];
                client.Headers["ClientSecret"] = requestHeaders["ClientSecret"];
            });

            if (tenantConfiguration != null)
            {
                _tenantConfigurations.Add(key, tenantConfiguration);
            }

            return tenantConfiguration;
        }


        public void EndRequest()
        {
        }

        public void ApplicationError(HttpApplication application, EventArgs e)
        {

        }
    }
}