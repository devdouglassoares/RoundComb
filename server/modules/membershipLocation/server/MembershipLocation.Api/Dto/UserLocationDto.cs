namespace MembershipLocation.Api.Dto
{
    public class UserLocationDto : UserLocationBase
    {
        public LocationTypeDto LocationType { get; set; }
    }
}