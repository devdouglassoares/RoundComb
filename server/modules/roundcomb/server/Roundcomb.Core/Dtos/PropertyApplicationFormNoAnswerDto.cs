using ProductManagement.Core.Dto;
using Membership.Core.Dto;
using Roundcomb.Core.Entities;

namespace Roundcomb.Core.Dtos
{
    public class PropertyApplicationFormNoAnswerDto : PropertyApplicationFormInstanceBase
    {
        public PropertyDto Property { get; set; }

        public UserPersonalInformation UserInformation { get; set; }
    }
}