using Core.DynamicProperties.Dtos;
using System;

namespace Membership.Library.Dto
{
    public class UserProfileModel
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string ContactName { get; set; }

        public string ProfilePhotoUrl { get; set; }

        public string MarriageStatus { get; set; }

        public string Gender { get; set; }

        public DateTimeOffset? Birthday { get; set; }

        public string ProfileSummary { get; set; }

        public string PhoneNumber { get; set; }

        public string ContactPhoneNumber { get; set; }

        public long? LocationId { get; set; }

        public string Address { get; set; }

        public string Address2 { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string ZipCode { get; set; }

        public string ModifiedBy { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }

        public DynamicPropertyValuesModel ExtendedProperties { get; set; }
    }
}