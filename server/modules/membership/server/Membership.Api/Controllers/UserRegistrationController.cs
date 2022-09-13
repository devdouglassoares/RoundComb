using Core.Exceptions;
using Core.WebApi.ActionFilters;
using Membership.Core.Contracts.AuthAttributes;
using Membership.Core.Dto;
using Membership.Core.Exceptions;
using Membership.Core.Models;
using Membership.Core.Permissions;
using Membership.Library.Contracts;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Membership.Api.Controllers
{
    [ErrorResponseHandler(typeof(InvalidOperationException), HttpStatusCode.BadRequest)]
    [ErrorResponseHandler(typeof(BaseNotFoundException<>), HttpStatusCode.NotFound)]
    [ErrorResponseHandler(typeof(UserEmailAlreadyInUsedException), HttpStatusCode.BadRequest)]
    public class UserRegistrationController : ApiController
    {
        private readonly IUserRegistrationService _userRegistrationService;

        public UserRegistrationController(IUserRegistrationService userRegistrationService)
        {
            _userRegistrationService = userRegistrationService;
        }

        [HttpPost, Route("api/registerUser")]
        public HttpResponseMessage RegisterUser(UserRegistrationModel model)
        {
            var userLoggedInToken = _userRegistrationService.Register(model, Request.RequestUri);

            return Request.CreateResponse(HttpStatusCode.OK, userLoggedInToken);
        }

        [HttpPost]
        [Route("api/activateUserViaToken")]
        public HttpResponseMessage ActivateUser(string token)
        {
            _userRegistrationService.ActivateUser(token);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost, Route("api/approveUser/{userId}")]
        [PermissionAuthorize(MembershipPermissions.ApproveUserRegistration)]
        public HttpResponseMessage ApproveUser(long userId)
        {
            _userRegistrationService.ApproveUser(userId);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost, Route("api/activateUserViaCode")]
        public HttpResponseMessage ActivateUser(UserTokenValidation model)
        {
            _userRegistrationService.ActivateUser(model);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
