using Membership.Core.Contracts.Common;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Membership.Core.Contracts.AuthAttributes
{
    public class RequireAuthTokenApiAttribute : AuthorizationFilterAttribute
    {
        public bool OptionalAuthorization { get; set; }

        //public StaticPermissions.Modules[] PageModules { get; set; }

        private IMembership membership
        {
            get
            {
                return ServiceLocator.Current.GetInstance<IMembership>();
            }
        }

        public override void OnAuthorization(HttpActionContext context)
        {
            bool skipAuthorization = context.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
                                     ||
                                     context.ControllerContext.ControllerDescriptor
                                            .GetCustomAttributes<AllowAnonymousAttribute>().Any();
            bool optionalAuthorization = this.OptionalAuthorization ||
                                         context.ActionDescriptor.GetCustomAttributes<OptionalAuthorizationAttribute>()
                                                .Any()
                                         ||
                                         context.ControllerContext.ControllerDescriptor
                                                .GetCustomAttributes<OptionalAuthorizationAttribute>().Any();

            if (skipAuthorization)
            {
                return;
            }

            string authToken = null;

            // first check the header
            if (context.Request.Headers.Any(h => h.Key == WebAuthorizeConst.AuthorizeTokenName))
            {
                authToken =
                    context.Request.Headers.First(h => h.Key == WebAuthorizeConst.AuthorizeTokenName)
                           .Value.FirstOrDefault();
            }

            // if authToken does not exists
            if (string.IsNullOrEmpty(authToken))
            {
                // angular2 http will append header name as lower cased, need to support it too.
                if (
                    context.Request.Headers.Any(
                               h =>
                                   string.Equals(h.Key, WebAuthorizeConst.AuthorizeTokenName,
                                                 StringComparison.CurrentCultureIgnoreCase)))
                {
                    authToken =
                        context.Request.Headers.First(
                                   h =>
                                       string.Equals(h.Key, WebAuthorizeConst.AuthorizeTokenName,
                                                     StringComparison.CurrentCultureIgnoreCase))
                               .Value.FirstOrDefault();
                }
            }

            // if header value still does not exist then check cookie
            if (string.IsNullOrEmpty(authToken))
            {
                var cookies = context.Request.Headers.GetCookies();
                if (cookies.Any())
                {
                    var cookie = cookies.First().Cookies.FirstOrDefault(c => c.Name == WebAuthorizeConst.AuthorizeTokenName);
                    if (cookie != null)
                    {
                        authToken = cookie.Value;
                    }
                }
            }

            var result = membership.ValidateUser(authToken);

            if (!result.IsSuccess && !optionalAuthorization)
            {
                context.Response = context.Request.CreateResponse(HttpStatusCode.Unauthorized, result.Errors.First());
            }
        }
    }
}