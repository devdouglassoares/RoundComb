using ProductManagement.Core.Dto;
using System;

namespace ProductManagement.Core.Models
{
    public class VendorRequestItemModel
    {
        public long Id { get; set; }
        public DateTimeOffset RequestSentDate { get; set; }
        public bool IsAccepted { get; set; }
        public DateTimeOffset? RequestAcceptedDate { get; set; }
        public long PropertyId { get; set; }
        public PropertyDto Property { get; set; }


        public long FromUserId { get; set; }

        /// <summary>
        /// From User Request
        /// </summary>
        public UserInformation UserInformation { get; set; }

        /// <summary>
        /// Vendor UserId
        /// </summary>
        public long ToUserId { get; set; }
        /// <summary>
        /// From Vendor UserId
        /// </summary>
        public VendorItemModel VendorInformation { get; set; }
    }
}
