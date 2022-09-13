using Membership.Core.Entities;
using Membership.Core.Entities.Base;

namespace Membership.Library.Entities
{
    public class CustomerVendor : BaseEntity
    {
        public virtual Company Company {get; set; }
        public virtual Vendor Vendor {get; set; }
        public virtual VendorType VendorType {get; set; }
        public virtual ProjectDescription ProjectDescription {get; set; }
        public virtual CustomerSite Site { get; set; }
        public int ExternalId { get; set; }
        public string Notes { get; set; }
    }
}
