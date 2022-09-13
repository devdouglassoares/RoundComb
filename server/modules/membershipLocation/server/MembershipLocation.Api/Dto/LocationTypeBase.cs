namespace MembershipLocation.Api.Dto
{
    public abstract class LocationTypeBase
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }
    }
}