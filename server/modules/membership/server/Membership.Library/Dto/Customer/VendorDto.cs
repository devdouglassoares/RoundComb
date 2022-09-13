
namespace Membership.Library.Dto.Customer
{
    public class VendorDto
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public long VendorId { get; set; }
        public string Name { get; set; }
        public long VendorTypeId { get; set; }
        public string Type { get; set; }
        public long ProjectDescriptionId { get; set; }
        public string ProjectDescription { get; set; }
        public long SiteId { get; set; }
        public int ExternalId { get; set; }
        public string Notes { get; set; }
    }
}
