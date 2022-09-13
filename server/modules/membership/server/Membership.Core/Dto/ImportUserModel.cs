namespace Membership.Core.Dto
{
    public class ImportUserDto
    {
        public string ExternalKey { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
    }
}
