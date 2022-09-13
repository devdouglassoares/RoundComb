using System.Collections.Generic;
using Membership.Core.Entities.Base;

namespace Membership.Core.Entities
{
    public class Group : BaseEntity
    {
        public Group()
        {
            Users = new HashSet<User>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public Company Company { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}