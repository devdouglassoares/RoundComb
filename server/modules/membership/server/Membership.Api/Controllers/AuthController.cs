using Core.Exceptions;
using Core.Logging;
using Core.WebApi.ActionFilters;
using Core.WebApi.Extensions;
using Membership.Api.Models;
using Membership.Core;
using Membership.Core.Contracts.AuthAttributes;
using Membership.Core.Contracts.Common;
using Membership.Core.Dto;
using Membership.Core.Entities;
using Membership.Core.Models;
using Membership.Core.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;

namespace Membership.Api.Controllers
{
	[ErrorResponseHandler(typeof(BaseNotFoundException<>), HttpStatusCode.NotFound)]
	[ErrorResponseHandler(typeof(InvalidOperationException), HttpStatusCode.BadRequest)]
	public class AuthController : ApiController
	{
		private readonly ILogger _logger = Logger.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly IMembership _membership;

		public AuthController(IMembership membership)
		{
			_membership = membership;
		}

		[HttpGet]
		[HttpHead]
		[Route("auth/user")]
		[RequireAuthTokenApi]
		public HttpResponseMessage GetUser()
		{
			return Request.CreateResponse(HttpStatusCode.OK, new
			{
				userName = _membership.Name,
				userId = _membership.UserId,
				externalId = _membership.UserExternalId,
				roles = _membership.GetCurrentUser().Roles.Select(x => x.Name.ToLower())
			}, _membership.GetCurrentUser().LastActivityDate);
		}

		[HttpPost]
		[Route("auth")]
		[ErrorResponseHandler(typeof(InvalidOperationException), HttpStatusCode.BadRequest)]
		[ErrorResponseHandler(typeof(UnauthorizedAccessException), HttpStatusCode.Unauthorized)]
		public IHttpActionResult Post(AuthModel model)
		{
			var membershipResult = _membership.ValidateUser(model.Login, model.Password);
			if (membershipResult.IsSuccess)
			{
				var token = _membership.CreateToken();
				return Ok(new UserLogInTokenModel
				{
					key = WebAuthorizeConst.AuthorizeTokenName,
					value = token,
					userName = _membership.Name
				});
			}
			throw new UnauthorizedAccessException(membershipResult.Errors.FirstOrDefault());
		}

		[HttpGet]
		[Route("auth/permissions")]
		[RequireAuthTokenApi]
		public IHttpActionResult GetPermissions()
		{
			if (_membership.IsSysAdmin)
			{
				return Ok(new List<string>
						  {
							  MembershipConstant.SysAdminPermission
						  });
			}

			var allowedPages = _membership.GetCurrentUserAllowedPages().ToList();
			var allowedFeatures = _membership.GetCurrentUserAllowedFeatures().ToList();

			allowedPages.AddRange(allowedFeatures);

			allowedPages.Add("/myaccount");

			if (_membership.GetCurrentUser().IsActive)
			{
				allowedPages.Add("authorizedUser");
			}

			if (_membership.IsImpersonated)
			{
				allowedPages.Add(MembershipConstant.CancelImpersonationPermission);
				//allowedPages.Add("/sysadmin");
			}

			return Ok(allowedPages);
		}

		[HttpPost, Route("auth/changePassword")]
		[RequireAuthTokenApi]
		[ErrorResponseHandler(typeof(BaseNotFoundException<User>), HttpStatusCode.Unauthorized)]
		[ErrorResponseHandler(typeof(InvalidOperationException), HttpStatusCode.BadRequest)]
		[ErrorResponseHandler(typeof(UnauthorizedAccessException), HttpStatusCode.Unauthorized)]
		public HttpResponseMessage ChangePassword(ChangePasswordModel model)
		{
			if (string.IsNullOrEmpty(model.OldPassword) || string.IsNullOrEmpty(model.NewPassword))
				throw new InvalidOperationException("Old password and new password must not be null");

			if (model.NewPassword != model.ConfirmNewPassword)
				throw new InvalidOperationException("New password and confirm password must match");

			if (!_membership.ValidateUser(_membership.Email, model.OldPassword).IsSuccess)
				throw new UnauthorizedAccessException("Old password does not match our record");

			_membership.ChangePassword(_membership.UserId, model.NewPassword);

			return Request.CreateResponse(HttpStatusCode.OK, new
			{
				key = WebAuthorizeConst.AuthorizeTokenName,
				value = _membership.CreateToken(),
				userName = _membership.Name
			});
		}

		[RequireAuthTokenApi]
		[HttpGet]
		public IHttpActionResult CancelImpersonation()
		{
			_membership.CancelImpersonation();
			return Ok();
		}

		[RequireAuthTokenApi]
		[HttpGet]
		public object Impersonate(long id)
		{
			if (_membership.IsAccessAllowed(PermissionAuthorize.Feature(MembershipPermissions.Impersonation)))
			{
				_membership.Impersonate(id);

				return Ok();
			}

			return BadRequest();
		}
	}
}