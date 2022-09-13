using Membership.Core.Entities.Base;

namespace Membership.Core.Entities
{
    public class UserAccessRight : BaseEntity
    {
        public virtual AccessModule AccessModule { get; set; }
        public virtual AccessRight AccessRight { get; set; }
        public virtual User User { get; set; }
    }
}