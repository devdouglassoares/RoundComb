using Membership.Core.Entities.Base;

namespace Membership.Library.Entities
{
    public class ProjectDescription : BaseEntity
    {
        public string Description { get; set; }
        public int ExternalId { get; set; }
    }
}
