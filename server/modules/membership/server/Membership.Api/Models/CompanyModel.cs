using Core.DynamicProperties.Dtos;
using Membership.Core.Dto;

namespace Membership.Api.Models
{
    public class CompanyModel
    {
        public long Id { get; set; }

        public string CompanyName { get; set; }

        public string Alias { get; set; }

        public string Domain { get; set; }

        public string LogoUrl { get; set; }

        public string Code { get; set; }

        public string Theme { get; set; }

        public UserBaseModel Owner { get; set; }

        public string Address { get; set; }

        public DynamicPropertyValuesModel ExtendedProperties { get; set; }
    }
}