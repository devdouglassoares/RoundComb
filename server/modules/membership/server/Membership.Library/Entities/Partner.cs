using Membership.Core.Entities.Base;

namespace Membership.Library.Entities
{
    public class Partner : BaseEntity
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int ExternalId { get; set; }
    }
}
