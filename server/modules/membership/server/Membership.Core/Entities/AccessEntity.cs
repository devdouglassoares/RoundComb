using System.Collections.Generic;
using Membership.Core.Entities.Base;
using Membership.Core.Entities.Enums;

namespace Membership.Core.Entities
{
    public class AccessEntity : BaseEntity
    {
        public AccessEntity()
        {
            AccessModules = new HashSet<AccessModule>();
        }

        public string Name { get; set; }

        public AccessEntityType Type { get; set; }

        public byte Status { get; set; }

        public virtual ICollection<AccessModule> AccessModules { get; set; }
    }
}