
using Core.DynamicProperties.Dtos;
using Membership.Core.Dto;
using System;

namespace Membership.Library.Dto
{
    public class CompanyDto
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string CompanyName { get; set; }

        public string Domain { get; set; }

        public string LogoUrl { get; set; }

        public string Code { get; set; }

        public string Alias { get; set; }

        public string Address { get; set; }

        public bool IsDeleted { get; set; }

        public string Status { get; set; }

        public DateTime? StatusLastUpdated { get; set; }

        public DateTime? StatusValidDate { get; set; }

        public bool IsStatusExpired { get; set; }

        public UserBaseModel Owner { get; set; }

        public UserBaseModel MainContactUser { get; set; }

        public DynamicPropertyValuesModel ExtendedProperties { get; set; }

        public long? MasterCompanyId { get; set; }

        public long? OwnerId { get; set; }

        public long? MainContactUserId { get; set; }
    }
}
