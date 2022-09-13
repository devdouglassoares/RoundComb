using System;

namespace Core.Database.Entities
{
    public abstract class BaseEntity
    {
        public long Id { get; set; }

        public bool IsDeleted { get; set; }

        public virtual DateTimeOffset? CreatedDate { get; set; }

        public virtual DateTimeOffset? ModifiedDate { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }
    }
}