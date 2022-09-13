
namespace Membership.Library.Dto.Customer
{
    public class SiteDto
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int? ZipCode { get; set; }
        public bool? IsPrimary { get; set; }
        public int ExternalId { get; set; }
    }
}
