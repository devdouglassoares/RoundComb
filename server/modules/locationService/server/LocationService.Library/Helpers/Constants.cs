namespace LocationService.Library.Helpers
{
    /**
     * Represents a point on the surface of a sphere. (The Earth is almost
     * spherical.)
     *
     * This code was originally published at
     * http://JanMatuschek.de/LatitudeLongitudeBoundingCoordinates#Java
     * 
     * @author Jan Philip Matuschek
     * @version 22 September 2010
     * @converted to C# by Anthony Zigenbine on 19th October 2010
     */

    public class Constants
    {
        public const double EarthRadiusMiles = 3959;
        public const double EarthRadiusInKilometers = 6371;
        public const double MilesToKilometers = 1.60934;
    }
}