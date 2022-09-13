
namespace Membership.Library.Dto.Customer
{
    public class PartnershipDto
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public long PartnerId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int ExternalId { get; set; }
    }
}
