using Membership.Core.Entities;
using Membership.Core.Entities.Base;

namespace Membership.Library.Entities
{
    public class ConnectionInfo : BaseEntity
    {
        public Company Company { get; set; }
        public string Content { get; set; }
    }
}
