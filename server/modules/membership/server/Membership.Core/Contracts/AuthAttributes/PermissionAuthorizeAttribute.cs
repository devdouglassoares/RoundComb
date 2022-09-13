using Membership.Core.Contracts.Common;
using Membership.Core.Models;
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
	public class PermissionAuthorizeAttribute : AuthorizationFilterAttribute
	{
		public string Permission { get; set; }

		public IMembership Membership
		{
			get { return ServiceLocator.Current.GetInstance<IMembership>(); }
		}

		public PermissionAuthorizeAttribute()
		{

		}

		public PermissionAuthorizeAttribute(string permission)
		{
			Permission = permission;
		}

		public override void OnAuthorization(HttpActionContext context)
		{
			bool skipAuthorization = context.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
									 ||
									 context.ControllerContext.ControllerDescriptor
											.GetCustomAttributes<AllowAnonymousAttribute>().Any();

			if (skipAuthorization)
			{
				return;
			}

			var authToken = GetAuthenticationToken(context);

			var isValid = Membership.ValidateUser(authToken).IsSuccess;

			if (!isValid)
			{
				context.Response = context.Request.CreateResponse(HttpStatusCode.Unauthorized, "Invalid authentification token");
			}

			if (Permission == null) return;

			if (!Membership.IsAccessAllowed(PermissionAuthorize.Feature(Permission)) &&
				!Membership.IsAccessAllowed(PermissionAuthorize.Page(Permission)))
			{
				context.Response = context.Request.CreateResponse(HttpStatusCode.Unauthorized,
					"You don't have permission to access this resource");
			}
		}

		private static string GetAuthenticationToken(HttpActionContext context)
		{
			string authToken = null;

			// first check the header
			if (context.Request.Headers.Any(h => string.Equals(h.Key, WebAuthorizeConst.AuthorizeTokenName, StringComparison.CurrentCultureIgnoreCase)))
			{
				authToken =
					context.Request.Headers.First(h => string.Equals(h.Key, WebAuthorizeConst.AuthorizeTokenName, StringComparison.CurrentCultureIgnoreCase)).Value.FirstOrDefault();
			}

			// if header value not exist then check cookie
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
			return authToken;
		}
	}
}