using Membership.Core.Entities;
using Membership.Core.Entities.Base;

namespace Membership.Library.Entities
{
    public class CustomerPartner : BaseEntity
    {
        public virtual Company Company {get; set; }
        public virtual Partner Partner { get; set; }
        public int ExternalId { get; set; }
    }
}
