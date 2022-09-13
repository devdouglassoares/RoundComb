using System;

namespace ProductManagement.Core.Dto
{
    public class PropertyCartItemDto
    {
        public long PropertyId { get; set; }

        public virtual PropertyDto Property { get; set; }

        public int Quantity { get; set; }

        public decimal? AppliedPrice { get; set; }

        public decimal? AppliedUnitPrice { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }
    }
}