using Core.Exceptions;
using Core.WebApi.ActionFilters;
using Core.WebApi.Extensions;
using Membership.Core;
using Membership.Core.Contracts.AuthAttributes;
using Membership.Library.Contracts;
using Membership.Library.Dto;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Membership.Api.Controllers
{
    [RequireAuthTokenApi]
    [ErrorResponseHandler(typeof(BaseNotFoundException<>), HttpStatusCode.NotFound)]
    public class UserProfileController : ApiController
    {
        private readonly IUserProfileService _profileService;
        private readonly IMembership _membership;

        public UserProfileController(IUserProfileService profileService, IMembership membership)
        {
            _profileService = profileService;
            _membership = membership;
        }

        [HttpGet, HttpHead]
        [Route("~/api/user/myprofile")]
        public HttpResponseMessage GetProfileForCurrentUser()
        {
            return GetProfileForUser(_membership.UserId);

        }

        [HttpPost, Route("~/api/user/myprofile")]
        public HttpResponseMessage SaveProfileForCurrentUser(UserProfileModel model)
        {
            _profileService.UpdateUserProfileForUser(_membership.UserId, model);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpGet, HttpHead, Route("~/api/user/{userId}/profile")]
        [AllowAnonymous]
        [ErrorResponseHandler(typeof(InvalidOperationException), HttpStatusCode.BadRequest)]
        public HttpResponseMessage GetProfileForUser(long userId)
        {
            var userProfile = _profileService.GetUserProfileForUser(userId);
            return Request.CreateResponse(HttpStatusCode.OK, userProfile, userProfile.ModifiedDate);
        }

        [HttpPost, Route("~/api/user/{userId}/profile")]
        [ErrorResponseHandler(typeof(InvalidOperationException), HttpStatusCode.BadRequest)]
        public HttpResponseMessage SaveProfileForUser(long userId, UserProfileModel model)
        {
            _profileService.UpdateUserProfileForUser(userId, model);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
