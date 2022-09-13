using Core.Database.Entities;
using System.Collections.Generic;

namespace ProductManagement.Core.Entities
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<Property> Properties { get; set; }

        public virtual ICollection<Tag> ChildrenTags { get; set; }

        public virtual Tag ParentTag { get; set; }
    }
}
