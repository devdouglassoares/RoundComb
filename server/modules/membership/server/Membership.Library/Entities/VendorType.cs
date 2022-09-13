using Membership.Core.Entities.Base;

namespace Membership.Library.Entities
{
    public class VendorType : BaseEntity
    {
        public string TypeName { get; set; }
        public int ExternalId { get; set; }
    }
}
