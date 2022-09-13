using ProductManagement.Core.Dto;
using Membership.Core.Dto;
using Roundcomb.Core.Base;

namespace Roundcomb.Core.Dtos
{
    public class PropertyCustomerConsumingMappingDto : PropertyCustomerConsumingMappingBase
    {
        public PropertyDto Property { get; set; }

        public UserPersonalInformation CustomerInformation { get; set; }
    }
}