using System;

namespace Membership.Core.Dto
{
    public class ClientCompanyDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string ContactName { get; set; }

        public string ContactPhoneNumber { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }
    }


    public class ClientCompanyWithOwnerDto : ClientCompanyDto
    {
        public UserBaseModel Owner { get; set; }

        public UserBaseModel MainContactUser { get; set; }
    }
}