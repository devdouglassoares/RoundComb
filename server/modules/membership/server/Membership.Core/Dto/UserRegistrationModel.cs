using System.Collections.Generic;

namespace Membership.Core.Dto
{
	public class UserRegistrationModel
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Email { get; set; }

		public string CellPhoneNumber { get; set; }

		public string HomePhoneNumber { get; set; }

		public string Password { get; set; }

		public List<long> Roles { get; set; }

        // just to simplify
        public string ExternalProvider { get; set; }
    }
}