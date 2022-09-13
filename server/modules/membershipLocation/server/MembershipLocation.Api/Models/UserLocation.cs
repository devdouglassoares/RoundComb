using MembershipLocation.Api.Dto;

namespace MembershipLocation.Api.Models
{
    public class UserLocation : UserLocationBase
    {
        public virtual LocationType LocationType { get; set; }
    }
}