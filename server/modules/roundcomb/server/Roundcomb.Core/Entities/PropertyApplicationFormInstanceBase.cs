using Core.Database.Entities;
using System;

namespace Roundcomb.Core.Entities
{
    public abstract class PropertyApplicationFormInstanceBase : BaseEntity
    {
		public string UploadedApplicationFileName { get; set; }

		public string UploadedApplicationFileUrl { get; set; }

	    public bool IsExternalSite { get; set; }

		public string ResultUrl { get; set; }

		public long FormInstanceId { get; set; }

        public long UserId { get; set; }

        public long PropertyId { get; set; }

        public bool IsApproved { get; set; }

        public DateTimeOffset? ApprovedDate { get; set; }

        public bool IsViewed { get; set; }

        public DateTimeOffset? ViewedDate { get; set; }

        public bool IsRejected { get; set; }

        public DateTimeOffset? RejectedDate { get; set; }

        public string RejectReason { get; set; }

        public string RejectedBy { get; set; }

        public bool UserAccepted { get; set; }

        public bool UserDeclined { get; set; }

        public DateTimeOffset? UserAcceptedDate { get; set; }

        public DateTimeOffset? UserDeclinedDate { get; set; }

        public string DeclinedReason { get; set; }
    }
}