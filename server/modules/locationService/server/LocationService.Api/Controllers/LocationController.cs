using Core.Exceptions;
using Core.WebApi;
using Core.WebApi.ActionFilters;
using LocationService.Library.Dtos;
using LocationService.Library.Helpers;
using LocationService.Library.Models;
using LocationService.Library.Services;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace LocationService.Api.Controllers
{
    [RoutePrefix("api/locations")]
    [ErrorResponseHandler(typeof(BaseNotFoundException<>), HttpStatusCode.NotFound)]
    public class LocationController : ApiController
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet, Route("getLocations/{ids}")]
        public HttpResponseMessage GetLocations([ModelBinder(typeof(CommaDelimitedArrayModelBinder))]long[] ids)
        {
            if (!ids.Any())
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Request location ids must be specified");

            if (ids.Length == 1)
            {
                var location = _locationService.GetEntity(ids.First());
                return Request.CreateResponse(HttpStatusCode.OK, location);
            }

            var locations = _locationService.Fetch(x => ids.Contains(x.Id));

            return Request.CreateResponse(HttpStatusCode.OK, locations);
        }

        [HttpGet, Route("locationBoundary")]
        public HttpResponseMessage GetLocationBoundary([FromUri]LocationQueryRequest model)
        {
            //var result = _locationService.GetLocationBoundary(model);
            var result = _locationService.GetLocationKmlFileLocation(model);
            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                kmlPath = result
            });
        }

        [HttpGet, Route("boundingLocations")]
        public HttpResponseMessage GetBoundingLocations(double lat, double lon, double distance, DistanceUnit unit = DistanceUnit.Kilometers)
        {
            var locations = _locationService.GetLocationsInDistance(lat, lon, distance, unit);

            return Request.CreateResponse(HttpStatusCode.OK, locations);
        }

        [HttpPost, Route("")]
        public HttpResponseMessage CreateLocation(LocationDto model)
        {
            var location = _locationService.CreateOrGet(model);
            return Request.CreateResponse(HttpStatusCode.OK, location);
        }

        [HttpPut, Route("{locationId:int}")]
        public HttpResponseMessage UpdateLocation(long locationId, LocationDto model)
        {
            var location = _locationService.Update(model, locationId);
            return Request.CreateResponse(HttpStatusCode.OK, location);
        }
    }
}
