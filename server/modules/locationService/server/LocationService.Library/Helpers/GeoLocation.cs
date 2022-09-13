using System;

namespace LocationService.Library.Helpers
{
    public class GeoLocation
    {
        public double Latitude { get; set; }  // latitude in radians

        public double Longitude { get; set; }  // longitude in radians

        public double DegreeLatitude { get; set; }  // latitude in degrees
        public double DegreeLongitude { get; set; }  // longitude in degrees

        private static readonly double MinLat = GeoUtils.ConvertDegreesToRadians(-90d);  // -PI/2
        private static readonly double MaxLat = GeoUtils.ConvertDegreesToRadians(90d);   //  PI/2
        private static readonly double MinLon = GeoUtils.ConvertDegreesToRadians(-180d); // -PI
        private static readonly double MaxLon = GeoUtils.ConvertDegreesToRadians(180d);  //  PI

        
        private GeoLocation()
        {
        }

        /// <summary>
        /// Return GeoLocation from Degrees
        /// </summary>
        /// <param name="latitude">The latitude, in degrees.</param>
        /// <param name="longitude">The longitude, in degrees.</param>
        /// <returns>GeoLocation in Degrees</returns>
        public static GeoLocation FromDegrees(double latitude, double longitude)
        {
            var result = new GeoLocation
            {
                Latitude = GeoUtils.ConvertDegreesToRadians(latitude),
                Longitude = GeoUtils.ConvertDegreesToRadians(longitude),
                DegreeLatitude = latitude,
                DegreeLongitude = longitude
            };
            result.CheckBounds();
            return result;
        }

        /// <summary>
        /// Return GeoLocation from Radians
        /// </summary>
        /// <param name="latitude">The latitude, in radians.</param>
        /// <param name="longitude">The longitude, in radians.</param>
        /// <returns>GeoLocation in Radians</returns>
        public static GeoLocation FromRadians(double latitude, double longitude)
        {
            var result = new GeoLocation
            {
                Latitude = latitude,
                Longitude = longitude,
                DegreeLatitude = GeoUtils.ConvertRadiansToDegrees(latitude),
                DegreeLongitude = GeoUtils.ConvertRadiansToDegrees(longitude)
            };
            result.CheckBounds();
            return result;
        }

        private void CheckBounds()
        {
            if (Latitude < MinLat || Latitude > MaxLat ||
                Longitude < MinLon || Longitude > MaxLon)
                throw new Exception("Arguments are out of bounds");
        }

        /// <summary>
        /// Computes the great circle distance between this GeoLocation instance and the location argument.
        /// </summary>
        /// <param name="location">Location to act as the centre point</param>
        /// <returns>the distance, measured in the same unit as the radius argument.</returns>
        public double DistanceTo(GeoLocation location)
        {
            return Math.Acos(Math.Sin(Latitude) * Math.Sin(location.Latitude) +
                             Math.Cos(Latitude) * Math.Cos(location.Latitude) *
                             Math.Cos(Longitude - location.Longitude)) * Constants.EarthRadiusInKilometers;
        }

        /// <summary>
        /// Computes the bounding coordinates of all points on the surface
        /// of a sphere that have a great circle distance to the point represented
        /// by this GeoLocation instance that is less or equal to the distance
        /// argument.
        /// For more information about the formulae used in this method visit
        /// http://JanMatuschek.de/LatitudeLongitudeBoundingCoordinates
        /// </summary>
        /// <param name="distance">The distance from the point represented by this 
        /// GeoLocation instance. Must me measured in the same unit as the radius argument.
        /// </param>
        /// <returns>An array of two GeoLocation objects such that:
        /// 
        /// a) The latitude of any point within the specified distance is greater
        /// or equal to the latitude of the first array element and smaller or
        /// equal to the latitude of the second array element.
        /// 
        /// b) If the longitude of the first array element is smaller or equal to
        /// the longitude of the second element, then: 
        /// the longitude of any point within the specified distance is greater
        /// or equal to the longitude of the first array element and smaller or
        /// equal to the longitude of the second array element.
        /// 
        /// c) If the longitude of the first array element is greater than the
        /// longitude of the second element (this is the case if the 180th
        /// meridian is within the distance), then:
        /// the longitude of any point within the specified distance is greater
        /// or equal to the longitude of the first array element
        /// or smaller or equal to the longitude of the second
        /// array element.</returns>
        public GeoLocation[] BoundingCoordinates(double distance)
        {

            if (distance < 0d)
                throw new Exception("Distance cannot be less than 0");

            // angular distance in radians on a great circle
            var radDist = distance / Constants.EarthRadiusInKilometers;

            var minLat = Latitude - radDist;
            var maxLat = Latitude + radDist;

            double minLon, maxLon;
            if (minLat > MinLat && maxLat < MaxLat)
            {
                var deltaLon = Math.Asin(Math.Sin(radDist) /
                                            Math.Cos(Latitude));
                minLon = Longitude - deltaLon;
                if (minLon < MinLon) minLon += 2d * Math.PI;
                maxLon = Longitude + deltaLon;
                if (maxLon > MaxLon) maxLon -= 2d * Math.PI;
            }
            else
            {
                // a pole is within the distance
                minLat = Math.Max(minLat, MinLat);
                maxLat = Math.Min(maxLat, MaxLat);
                minLon = MinLon;
                maxLon = MaxLon;
            }

            return new[]
                   {
                       FromRadians(minLat, minLon),
                       FromRadians(maxLat, maxLon)
                   };
        }
    }
}