using System;

namespace ProductManagement.Core.Entities
{
    public class PropertyCartItem
    {
        public long PropertyId { get; set; }

        public virtual Property Property { get; set; }

        public long PropertyCartId { get; set; }

        public virtual PropertyCart PropertyCart { get; set; }

        public int Quantity { get; set; }

        public decimal? AppliedPrice { get; set; }

        public decimal? AppliedUnitPrice { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }
    }
}