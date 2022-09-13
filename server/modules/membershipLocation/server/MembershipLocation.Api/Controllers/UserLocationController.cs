using Membership.Core;
using Membership.Core.Contracts.AuthAttributes;
using MembershipLocation.Api.Services;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MembershipLocation.Api.Controllers
{
    [RoutePrefix("api/userLocation")]
    public class UserLocationController : ApiController
    {
        private readonly IMembership _membership;
        private readonly IUserLocationService _userLocationService;

        public UserLocationController(IMembership membership, IUserLocationService userLocationService)
        {
            _membership = membership;
            _userLocationService = userLocationService;
        }

        [HttpGet, Route("")]
        [RequireAuthTokenApi(OptionalAuthorization = true)]
        public HttpResponseMessage GetCurrentUserLocations(long? userId)
        {
            var userLocationDtos = _userLocationService.GetUserLocations(userId ?? _membership.UserId);
            return Request.CreateResponse(HttpStatusCode.OK, userLocationDtos);
        }

        [HttpGet, Route("{locationId:long}")]
        [RequireAuthTokenApi(OptionalAuthorization = true)]
        public HttpResponseMessage GetUserLocation(long locationId)
        {
            var userLocationDto = _userLocationService.GetUserLocation(_membership.UserId, locationId);
            return Request.CreateResponse(HttpStatusCode.OK, userLocationDto);
        }

        [HttpDelete, Route("{locationId:long}")]
        [RequireAuthTokenApi]
        public HttpResponseMessage DeleteUserLocation(long locationId)
        {
            _userLocationService.DeleteUserLocation(_membership.UserId, locationId);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost, Route("{locationId:long}")]
        [RequireAuthTokenApi]
        public HttpResponseMessage AssignLocationToUser(long locationId, long? locationTypeId = null, bool isDefault = false)
        {
            _userLocationService.AssignLocation(_membership.UserId, locationId, locationTypeId, isDefault);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
