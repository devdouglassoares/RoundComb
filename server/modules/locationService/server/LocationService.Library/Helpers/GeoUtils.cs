using System;

namespace LocationService.Library.Helpers
{
    public class GeoUtils
    {
        public static double ConvertDegreesToRadians(double degrees)
        {
            return (Math.PI / 180) * degrees;
        }

        public static double ConvertRadiansToDegrees(double radian)
        {
            return radian * (180.0 / Math.PI);
        }
    }
}