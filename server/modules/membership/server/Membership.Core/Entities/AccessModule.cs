using System.Collections.Generic;
using Membership.Core.Entities.Base;

namespace Membership.Core.Entities
{
    public class AccessModule : BaseEntity
    {
        public AccessModule()
        {
            RoleAccessRights = new HashSet<RoleAccessRight>();
            UserAccessRights = new HashSet<UserAccessRight>();
            AccessEntities = new HashSet<AccessEntity>();   
        }

        public string Name { get; set; }
        public byte Status { get; set; }

        public virtual ICollection<RoleAccessRight> RoleAccessRights { get; set; }
        public virtual ICollection<UserAccessRight> UserAccessRights { get; set; }
        public virtual ICollection<AccessEntity> AccessEntities { get; set; }
    }
}