using System.Linq;
using System.Web.Mvc;
using Membership.Core.Contracts.Common;
using Microsoft.Practices.ServiceLocation;

namespace Membership.Core.Contracts.AuthAttributes
{
    public class RequireAuthTokenMvcAttribute : ActionFilterAttribute
    {
        public bool OptionalAuthorization { get; set; }

        private IMembership membership
        {
            get
            {
                return ServiceLocator.Current.GetInstance<IMembership>();
            }
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            bool skipAuthorization = context.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any();
            bool optionalAuthorization = this.OptionalAuthorization || context.ActionDescriptor.GetCustomAttributes(typeof(OptionalAuthorizationAttribute), true).Any();

            if (skipAuthorization)
            {
                return;
            }

            // first check the header
            var authToken = context.HttpContext.Request.Headers.Get(WebAuthorizeConst.AuthorizeTokenName);

            // if header value not exist then check cookie
            if (authToken == null)
            {
                var cookie = context.HttpContext.Request.Cookies.Get(WebAuthorizeConst.AuthorizeTokenName);
                if (cookie != null)
                {
                    authToken = cookie.Value;
                }
            }

            var result = membership.ValidateUser(authToken);
            
            if (!result.IsSuccess && !optionalAuthorization)
            {
                if (context.HttpContext.Request.IsAjaxRequest())
                {
                    context.Result = new HttpUnauthorizedResult(result.Errors.First());
                }
                else
                {
                    context.Result = new RedirectToRouteResult("auth", null);
                }
            }
        }
    }
}