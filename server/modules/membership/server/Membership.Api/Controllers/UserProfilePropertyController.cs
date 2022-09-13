using Core.DynamicProperties.Controllers;
using Core.WebApi.Extensions;
using Membership.Core;
using Membership.Core.Contracts.AuthAttributes;
using Membership.Core.Entities;
using Membership.Library.Services;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Membership.Api.Controllers
{
    [RoutePrefix("api/userProfileProperty")]
    public class UserProfilePropertyController : DynamicPropertiesController<UserProfile, IUserProfileDynamicPropertyService>
    {
        private readonly IUserProfileDynamicPropertyService _dynamicPropService;
        private readonly IMembership _membership;

        public UserProfilePropertyController(IUserProfileDynamicPropertyService dynamicPropService,
                                             IMembership membership) : base(dynamicPropService)
        {
            _dynamicPropService = dynamicPropService;
            _membership = membership;
        }

        [HttpGet, HttpHead, Route("myprofile")]
        [RequireAuthTokenApi]
        public HttpResponseMessage GetAllForCurrentUser()
        {
            return GetAllForUser(_membership.UserId);
        }

        [HttpGet, HttpHead, Route("{userId}/profile")]
        [RequireAuthTokenApi]
        public HttpResponseMessage GetAllForUser(long userId)
        {
            var allProfilePropertiesForUser = _dynamicPropService.GetAllProfilePropertiesForUser(userId).ToArray();

            return Request.CreateResponse(HttpStatusCode.OK, allProfilePropertiesForUser, allProfilePropertiesForUser.Max(x => x.ModifiedDate));
        }
    }
}
