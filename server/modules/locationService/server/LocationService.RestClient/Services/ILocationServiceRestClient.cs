using Core;
using Core.SiteSettings;
using LocationService.RestClient.Models;
using LocationService.RestClient.SiteSettings;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace LocationService.RestClient.Services
{
    public interface ILocationServiceRestClient : IDependency
    {
        IEnumerable<Location> GetLocations(long[] ids);

        IEnumerable<Location> GetLocationsWithinDistance(double lat, double lon, double distance);

        Location CreateLocation(Location model);

        Location UpdateLocation(long locationId, Location model);
    }

    public class LocationServiceRestClient : ILocationServiceRestClient
    {
        public readonly string ApiKey;
        public readonly string ApiSecret;

        private readonly RestSharp.RestClient _restClient;

        public LocationServiceRestClient(ISiteSettingService settingService)
        {
            var locationServiceIntegrationSetting = settingService.GetSetting<LocationServiceIntegrationSetting>();

            var locationServiceServer = locationServiceIntegrationSetting.LocationServiceServer;

            if (string.IsNullOrEmpty(locationServiceServer))
                throw new InvalidOperationException("LocationService configuration missing");

            ApiKey = locationServiceIntegrationSetting.LocationServiceApiKey;
            ApiSecret = locationServiceIntegrationSetting.LocationServiceApiSecret;


            _restClient = new RestSharp.RestClient(locationServiceServer);
        }

        public IEnumerable<Location> GetLocations(long[] ids)
        {
            var restRequest = GenerateRequest(Method.GET, "api/locations/" + string.Join(",", ids));

            var restResponse = _restClient.Execute(restRequest);

            if (restResponse.StatusCode != HttpStatusCode.OK)
                throw new HttpListenerException((int)restResponse.StatusCode, restResponse.ErrorMessage ?? restResponse.Content);

            if (ids.Length == 1)
            {
                var result = JsonConvert.DeserializeObject<Location>(restResponse.Content);
                return new[] { result };
            }
            return JsonConvert.DeserializeObject<IEnumerable<Location>>(restResponse.Content);
        }

        public IEnumerable<Location> GetLocationsWithinDistance(double lat, double lon, double distance)
        {
            var restRequest = GenerateRequest(Method.GET,
                                              $"api/locations/boundingLocations/{lat},{lon}_within_{distance}");

            var restResponse = _restClient.Execute(restRequest);

            if (restResponse.StatusCode != HttpStatusCode.OK)
                throw new HttpListenerException((int)restResponse.StatusCode, restResponse.ErrorMessage ?? restResponse.Content);

            return JsonConvert.DeserializeObject<IEnumerable<Location>>(restResponse.Content);
        }

        public Location CreateLocation(Location model)
        {
            var restRequest = GenerateRequest(Method.POST, $"api/locations");

            restRequest.AddBody(model);

            var restResponse = _restClient.Execute(restRequest);

            if (restResponse.StatusCode != HttpStatusCode.OK && restResponse.StatusCode != HttpStatusCode.Created)
                throw new HttpListenerException((int)restResponse.StatusCode, restResponse.ErrorMessage ?? restResponse.Content);

            return JsonConvert.DeserializeObject<Location>(restResponse.Content);
        }

        public Location UpdateLocation(long locationId, Location model)
        {
            var restRequest = GenerateRequest(Method.PUT, $"api/locations/{locationId}");

            restRequest.AddBody(model);

            var restResponse = _restClient.Execute(restRequest);

            if (restResponse.StatusCode != HttpStatusCode.OK && restResponse.StatusCode != HttpStatusCode.Created)
                throw new HttpListenerException((int)restResponse.StatusCode, restResponse.ErrorMessage ?? restResponse.Content);

            return JsonConvert.DeserializeObject<Location>(restResponse.Content);
        }

        private RestRequest GenerateRequest(Method method, string uriString)
        {
            var restRequest = new RestRequest(new Uri(uriString, UriKind.Relative), method);
            restRequest.AddHeader("AppId", ApiKey);
            restRequest.AddHeader("AppSecret", ApiSecret);
            return restRequest;
        }
    }
}