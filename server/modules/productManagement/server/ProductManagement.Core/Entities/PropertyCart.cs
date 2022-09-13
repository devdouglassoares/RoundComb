using Core.Database.Entities;
using System;
using System.Collections.Generic;

namespace ProductManagement.Core.Entities
{
    public class PropertyCart : BaseEntity
    {
        public long UserId { get; set; }

        public long? DelegateToUserId { get; set; }

        public bool CheckedOut { get; set; }

        public DateTimeOffset? CheckedOutDate { get; set; }

        public bool Closed { get; set; }

        public virtual ICollection<PropertyCartItem> Items { get; set; }
    }
}