using Membership.Core.Entities.Base;
using System;

namespace Membership.Core.Entities
{
    [Obsolete("Not maintained any more")]
    public class CustomerContact : BaseEntity
    {
        public virtual Company Company { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public int ExternalId { get; set; }

        public string Description { get; set; }
    }
}
