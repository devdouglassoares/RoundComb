using Membership.Core.Entities.Base;
using Membership.Core.Entities.Enums;

namespace Membership.Core.Entities
{
    public class AccessRight : BaseEntity
    {
        public string AccessRightName { get; set; }
        public int Priority { get; set; }
        public AccessKind AccessKind { get; set; }
    }
}