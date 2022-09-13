using Membership.Core.Entities.Base;

namespace Membership.Core.Entities
{
    public class RoleAccessRight : BaseEntity
    {
        public virtual AccessModule AccessModule { get; set; }
        public virtual AccessRight AccessRight { get; set; }
        public virtual Role Role { get; set; }
    }
}