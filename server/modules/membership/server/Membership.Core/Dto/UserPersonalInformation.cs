using System;
using System.Collections.Generic;

namespace Membership.Core.Dto
{
    public class UserPersonalInformation
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string CompanyName { get; set; }

        public string CellPhoneNumber { get; set; }

        public string HomePhoneNumber { get; set; }

        public long? CompanyId { get; set; }

        public List<string> Roles { get; set; }

        public List<string> RoleCodes { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }
    }
}