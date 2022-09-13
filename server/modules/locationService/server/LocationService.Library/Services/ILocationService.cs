using Core;
using Core.Database;
using Core.ObjectMapping;
using LocationService.Library.Data;
using LocationService.Library.Dtos;
using LocationService.Library.Helpers;
using LocationService.Library.Models;
using SharpKml.Base;
using SharpKml.Dom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using Location = LocationService.Library.Entities.Location;

namespace LocationService.Library.Services
{
    public interface ILocationService : IBaseService<Location, LocationDto>, IDependency
    {
        IQueryable<Location> GetLocationsInDistance(double sourceLat, double sourceLong, double distance,
                                                   DistanceUnit unit = DistanceUnit.Kilometers);

        IEnumerable<long> GetLocationsByKeyword(string keyword);

        IEnumerable<long> GetLocationsByKeyword(LocationQueryRequest keyword);

        Location CreateOrGet(LocationDto model);

        string GetLocationKmlFileContent(string locationName);

        string GetLocationKmlFileLocation(LocationQueryRequest model);

        IEnumerable<IEnumerable<double[]>> GetLocationBoundary(LocationQueryRequest model);
    }

    public class LocationService : BaseService<Location, LocationDto>, ILocationService
    {
        public LocationService(IMappingService mappingService, IRepository repository)
            : base(mappingService, repository)
        {
        }

        public IEnumerable<long> GetLocationsByKeyword(string keyword)
        {
            var keywords = keyword.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

            var locations = Repository.Fetch<Location>(location =>
                                                      keywords.Any(x => location.Address.ToLower().Contains(x.ToLower())) ||

                                                      keywords.Any(x => location.City.ToLower().Contains(x.ToLower())) ||

                                                      keywords.Any(x => location.Country.ToLower().Contains(x.ToLower())) ||

                                                      keywords.Any(x => location.State.ToLower().Contains(x.ToLower())) ||

                                                      keywords.Any(x => location.ZipCode.ToLower().Contains(x.ToLower()))
                );

            return locations.Select(x => x.Id).ToArray();
        }

        public IEnumerable<long> GetLocationsByKeyword(LocationQueryRequest keyword)
        {
            var locations = Repository.Fetch<Location>(location =>
                                                      (string.IsNullOrEmpty(keyword.StreetAddress.Trim()) ||
                                                       location.Address.ToLower()
                                                               .Contains(keyword.StreetAddress.Trim().ToLower())) &&

                                                      (string.IsNullOrEmpty(keyword.City.Trim()) ||
                                                       location.City.ToLower().Contains(keyword.City.Trim().ToLower())) &&

                                                      (string.IsNullOrEmpty(keyword.State.Trim()) ||
                                                       location.State.ToLower().Contains(keyword.State.Trim().ToLower())) &&

                                                      (string.IsNullOrEmpty(keyword.Country.Trim()) ||
                                                       location.Country.ToLower()
                                                               .Contains(keyword.Country.Trim().ToLower())) &&

                                                      (string.IsNullOrEmpty(keyword.ZipCode.Trim()) ||
                                                       location.ZipCode.ToLower()
                                                               .Contains(keyword.ZipCode.Trim().ToLower()))
                );

            return locations.Select(x => x.Id).ToArray();
        }

        public Location CreateOrGet(LocationDto model)
        {
            var result = Repository.First<Location>(location => location.FormattedAddress == model.FormattedAddress);

            return result ?? Create(model);
        }

        public virtual IEnumerable<IEnumerable<double[]>> GetLocationBoundary(LocationQueryRequest model)
        {
            var locationName = !string.IsNullOrEmpty(model.ZipCode) ?
                $"{model.Country}\\postalcodes\\{model.ZipCode}" :
                $"{model.Country}\\{model.State}" +
                               (string.IsNullOrEmpty(model.City) ? "" : $".{model.City}");

            var fileContent = GetLocationKmlFileContent(locationName);
            var kmlContent = new Parser();

            kmlContent.ParseString(fileContent, false);
            var rootPlaceMark = (Placemark)kmlContent.Root;
            if (rootPlaceMark == null)
                throw new InvalidOperationException("Kml file format is not valid");

            var geometryCollection = rootPlaceMark.Geometry as MultipleGeometry;
            var geometryList = new List<List<double[]>>();

            if (geometryCollection != null)
            {
                foreach (var geometry in geometryCollection.Geometry)
                {
                    var polygon = (Polygon)geometry;
                    geometryList.Add(
                                     new List<double[]>(
                                         polygon.OuterBoundary.LinearRing.Coordinates.Select(
                                                                                             x =>
                                                                                             new[]
                                                                                             {
                                                                                                 x.Latitude,
                                                                                                 x.Longitude
                                                                                             })));
                }
                return geometryList;
            }

            var singleGeometry = rootPlaceMark.Geometry as Polygon;

            if (singleGeometry != null)
            {
                geometryList.Add(
                                 new List<double[]>(singleGeometry.OuterBoundary.LinearRing.Coordinates
                                                                  .Select(
                                                                          x =>
                                                                          new[]
                                                                          {
                                                                              x.Latitude,
                                                                              x.Longitude
                                                                          })));
            }

            return geometryList;
        }

        public string GetLocationKmlFileContent(string locationName)
        {
            var basePath = HttpContext.Current != null
                               ? HostingEnvironment.MapPath("~/KmlFiles")
                               : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "KmlFiles");

            var fileToRead = Path.Combine(basePath, locationName + ".kml");

            if (!File.Exists(fileToRead))
                throw new InvalidOperationException("Location Geometry data is not available");

            var fileContent = File.ReadAllText(fileToRead);
            return fileContent;
        }

        public string GetLocationKmlFileLocation(LocationQueryRequest model)
        {
            var locationName = !string.IsNullOrEmpty(model.ZipCode) ?
                $"KmlFiles/{model.Country}/postalcodes/{model.ZipCode}" :

                $"KmlFiles/{model.Country}/{model.State}" +
                               (string.IsNullOrEmpty(model.City) ?
                               (string.IsNullOrEmpty(model.County) ? "" : $".{model.County}") :
                               $".{model.City}");
            return locationName + ".kml";
        }

        public IQueryable<Location> GetLocationsInDistance(double sourceLat, double sourceLong, double distance,
                                                          DistanceUnit unit = DistanceUnit.Kilometers)
        {
            var geoLocation = GeoLocation.FromDegrees(sourceLat, sourceLong);

            var distanceInKm = unit == DistanceUnit.Kilometers
                                   ? distance
                                   : distance * Constants.MilesToKilometers;



            var locationsWithinDistance = geoLocation.BoundingCoordinates(distanceInKm);

            var firstLat = locationsWithinDistance[0].DegreeLatitude;
            var secondLat = locationsWithinDistance[1].DegreeLatitude;

            var firstLon = locationsWithinDistance[0].DegreeLongitude;
            var secondLon = locationsWithinDistance[1].DegreeLongitude;

            var results = Repository.Fetch<Location>(x => !x.IsDeleted &&

                                                         x.Latitude >= firstLat &&
                                                         x.Latitude <= secondLat &&

                                                         (firstLon <= secondLon
                                                              ? x.Longitude >= firstLon && x.Longitude <= secondLon
                                                              : x.Longitude <= firstLon && x.Longitude >= secondLon
                                                         ));

            return results;
        }
    }
}