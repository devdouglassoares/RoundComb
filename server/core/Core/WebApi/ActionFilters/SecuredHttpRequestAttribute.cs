using Core.Logging;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Core.WebApi.ActionFilters
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class SecuredHttpRequestAttribute : AuthorizationFilterAttribute
    {
        private readonly ILogger _logger = Logger.GetLogger<SecuredHttpRequestAttribute>();

        public override void OnAuthorization(HttpActionContext actionContext)
        {

            var enableSecuredHttpRequests = ConfigurationManager.AppSettings["EnableSecuredHttpRequests"];

            if (string.IsNullOrEmpty(enableSecuredHttpRequests) || enableSecuredHttpRequests.ToLower() == "false")
            {
                base.OnAuthorization(actionContext);
                return;
            }

            var allowDomainsConfig = ConfigurationManager.AppSettings["AllowedDomains"] ?? "";
            var tokenConfiguration = ConfigurationManager.AppSettings["DomainAuthorizeToken"];
            var allowedAppIdsConfig = ConfigurationManager.AppSettings["AllowedAppIds"] ?? "";
            var appIdsTokenConfiguration = ConfigurationManager.AppSettings["AppIdAuthorizeToken"];


            var allowedSiteDomains = allowDomainsConfig.Split(new[] { ",", ";" },
                                                                    StringSplitOptions.RemoveEmptyEntries);

            var allowedAppIds = allowedAppIdsConfig.Split(new[] { ",", ";" },
                                                                    StringSplitOptions.RemoveEmptyEntries);


            // web request, reading from Origin header
            if (actionContext.Request.Headers.Contains("Origin"))
            {
                AuthenticateWebRequest(actionContext, allowedSiteDomains, tokenConfiguration);
            }
            else // mobile app request, reading from UserAgent
            {
                var userAgent = actionContext.Request.Headers.UserAgent;
                var userAgentString = userAgent.ToString();

                if (allowedAppIds.All(appId => !userAgentString.ToLower().Contains(appId.ToLower())))
                {
                    var logMessage =
                        $"A non-allowed app id is trying to connect to URL: {actionContext.Request.RequestUri} from {userAgentString}";

                    _logger.Warn(logMessage);
                    var errorString = "The request is declined due to security issue";
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,
                                                                                  errorString);
                    return;
                }


	            var tokenValid = ValidateToken(actionContext, appIdsTokenConfiguration);
				if (!tokenValid)
	            {
		            var logMessage =
			            $"A non-allowed site is trying to connect to URL: {actionContext.Request.RequestUri} from {userAgent}";

		            _logger.Warn(logMessage);
		            var errorString = "The request is declined due to security issue";
		            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, errorString);
		            return;
	            }
				base.OnAuthorization(actionContext);
            }
        }

        private void AuthenticateWebRequest(HttpActionContext actionContext, string[] allowedSiteDomains,
                                            string tokenConfiguration)
        {
            Uri uriResult = null;
            var origin = actionContext.Request.Headers.GetValues("Origin").FirstOrDefault();
            var isUrlValid = Uri.TryCreate(origin, UriKind.Absolute, out uriResult)
                              && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);


            if (!isUrlValid)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                return;
            }

            var isSiteAllowed = allowedSiteDomains.Contains(uriResult.Host);

            if (!isSiteAllowed)
            {
                var wildcardDomains = allowedSiteDomains.Where(x => x.StartsWith("*"));

                isSiteAllowed = wildcardDomains.Any(domain => uriResult.Host.EndsWith(domain.Replace("*", "")));
            }

            var tokenValid = ValidateToken(actionContext, tokenConfiguration);

	        if (!isSiteAllowed || !tokenValid)
            {
                var logMessage =
                    $"A non-allowed site is trying to connect to URL: {actionContext.Request.RequestUri} from {uriResult}";

                _logger.Warn(logMessage);
                var errorString = "The request is declined due to security issue";
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, errorString);
                return;
            }

            base.OnAuthorization(actionContext);
        }

	    private static bool ValidateToken(HttpActionContext actionContext, string tokenConfiguration)
	    {
		    var tokenValid = actionContext.Request.Headers.Contains("DomainAuthorizationToken");

		    if (tokenValid)
		    {
			    var token = actionContext.Request.Headers.GetValues("DomainAuthorizationToken").FirstOrDefault();
			    tokenValid = !string.IsNullOrEmpty(token) && token.Equals(tokenConfiguration);
		    }
		    return tokenValid;
	    }
    }
}