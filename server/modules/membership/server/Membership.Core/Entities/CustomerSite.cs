using Membership.Core.Entities.Base;

namespace Membership.Core.Entities
{
    public class CustomerSite : BaseEntity
    {
        public virtual Company Company { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int? Zipcode { get; set; }
        public bool? IsPrimary { get; set; }
    }
}
