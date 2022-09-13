namespace LocationService.Library.Models
{
    public class LocationQueryRequest
    {
        public string StreetAddress { get; set; }

        public string City { get; set; }

        public string County { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string ZipCode { get; set; }
    }
}