using System.Collections.Generic;
using Membership.Core.Entities.Base;

namespace Membership.Core.Entities
{
    public class Role : BaseEntity
    {
        public Role()
        {
            RoleAccessRights = new HashSet<RoleAccessRight>();
            Users = new HashSet<User>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<RoleAccessRight> RoleAccessRights { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}