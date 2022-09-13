using System;

namespace ProductManagement.Core.Entities
{
    public class VendorRequest
    {
        public long Id { get; set; }
        public long FromUserId { get; set; }
        public long ToUserId { get; set; }
        public long PropertyId { get; set; }
        public virtual Property Property { get; set; }
        public DateTimeOffset RequestSentDate { get; set; }
        public bool IsAccepted { get; set; }
        public DateTimeOffset? RequestAcceptedDate { get; set; }
    }
}
