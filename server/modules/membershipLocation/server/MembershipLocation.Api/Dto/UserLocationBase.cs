namespace MembershipLocation.Api.Dto
{
    public abstract class UserLocationBase
    {
        public long UserId { get; set; }

        public long LocationId { get; set; }

        public long? LocationTypeId { get; set; }

        public bool IsDefaultAddress { get; set; }
    }
}