using Membership.Core.Dto;

namespace Membership.Library.Dto
{
    public class UserWithProfileModel : UserBaseModel
    {

        public string ContactName { get; set; }

        public string PhoneNumber { get; set; }

        public string ContactPhoneNumber { get; set; }

        public bool NoEmailNeeded { get; set; }
    }
}