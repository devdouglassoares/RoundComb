using System;

namespace LocationService.Library.Helpers
{
    public static class GeoFormula
    {
        /// <summary>
        ///     This method calculates the distance between two geo points using Haversine formula
        ///     Refernce (http://en.wikipedia.org/wiki/Haversine_formula)
        /// </summary>
        public static double DistanceBetweenGeoPoints(double sourceLatitude, double sourceLongitude,
                                                      double destinationLatitude, double destinationLongitude,
                                                      DistanceUnit distanceUnit = DistanceUnit.Kilometers)
        {
            //Calculate difference between the latitude and longitude
            var deltaLongitude = destinationLongitude - sourceLongitude;
            var deltaLatitude = destinationLatitude - sourceLatitude;

            //Convert the values from degree to radian
            var deltaLatitudeRad = DegreeToRadian(deltaLatitude);
            var deltaLongitudeRad = DegreeToRadian(deltaLongitude);
            var sourceLatitudeRad = DegreeToRadian(sourceLatitude);
            var destinationLatitudeRad = DegreeToRadian(destinationLatitude);

            //Calculate haversine of the central angle between them
            var haversine = Math.Sin(deltaLatitudeRad / 2) * Math.Sin(deltaLatitudeRad / 2) +
                            Math.Cos(sourceLatitudeRad) * Math.Cos(destinationLatitudeRad) *
                            (Math.Sin(deltaLongitudeRad / 2) * Math.Sin(deltaLongitudeRad / 2));

            //Calculate Distance
            double distance = 0;

            switch (distanceUnit)
            {
                case DistanceUnit.Kilometers:
                    distance = (2 * Constants.EarthRadiusInKilometers) * Math.Asin(Math.Sqrt(haversine));
                    break;

                case DistanceUnit.Miles:
                    distance = (2 * Constants.EarthRadiusMiles) * Math.Asin(Math.Sqrt(haversine));
                    break;

                default:
                    distance = (2 * Constants.EarthRadiusInKilometers) * Math.Asin(Math.Sqrt(haversine));
                    break;
            }

            return distance;
        }

        private static double DegreeToRadian(double degree)
        {
            return (degree * Math.PI / 180);
        }
    }
}