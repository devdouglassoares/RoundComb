
namespace Membership.Library.Dto.Customer
{
    public class ContactDto
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int ExternalId { get; set; }
        public string Description { get; set; }
    }
}
