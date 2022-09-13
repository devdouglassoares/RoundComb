namespace Membership.Core.Dto
{
    public class ContactDto
    {
        public long Id { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }

        public long UserId { get; set; }

        public UserPersonalInformation User { get; set; }
    }
}