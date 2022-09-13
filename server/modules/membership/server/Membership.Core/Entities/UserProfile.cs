using Core.DynamicProperties.Models;
using Membership.Core.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Membership.Core.Entities
{
    public class UserProfile : BaseEntity, IHasDynamicProperty
    {
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

        public string CreatedBy { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }

        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}