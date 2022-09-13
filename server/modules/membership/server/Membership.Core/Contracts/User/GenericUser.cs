using System.Collections.Generic;

namespace Membership.Core.Contracts.User
{
    public class GenericUser
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string CompanyKey { get; set; }
        public List<string> Role { get; set; }

        //==================================
        public string Ssn { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsNewClient { get; set; }
        public string Comment { get; set; }
        public bool IsActive { get; set; }

        //public long? BizOwnerId { get; set; }
    }
}
