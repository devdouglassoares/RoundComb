namespace LocationService.Library.Dtos
{
    public class LocationDto
    {
        public long Id { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string ZipCode { get; set; }

        public string FormattedAddress { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string StateCode { get; set; }

        public string CountryCode { get; set; }
    }
}