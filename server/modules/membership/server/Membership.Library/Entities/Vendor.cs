using Membership.Core.Entities.Base;

namespace Membership.Library.Entities
{
    public class Vendor : BaseEntity
    {
        public string Name { get; set; }
        public int ExternalId { get; set; }
    }
}
