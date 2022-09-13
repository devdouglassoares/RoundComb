using Core.DynamicProperties.Dtos;
using Membership.Core.Dto;
using System;

namespace CustomForm.Core.Dtos
{
    public class FormInstanceDto
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public UserPersonalInformation UserInformation { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }

        public long FormConfigurationId { get; set; }

        public FormConfigurationDto FormConfiguration { get; set; }

        public DynamicPropertyValuesModel Answers { get; set; }

        public DynamicPropertyValuesModel ExtendedAnswers { get; set; }
    }
}