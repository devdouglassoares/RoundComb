using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityReviews.Models
{
    public class EntityReview
    {
        [Key]
        public long Id { get; set; }

        public long? ReviewerUserId { get; set; }

        public long TargetEntityId { get; set; }

        public string TargetEntityObject { get; set; }

        public long? ConnectedEntityId { get; set; }

        [MaxLength(256)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string ReviewText { get; set; }

        public int Rating { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }

        public bool IsApproved { get; set; }

        public DateTimeOffset? ApprovedDate { get; set; }

        public string ApprovedBy { get; set; }

        public int LikesCount { get; set; }

        public int DislikesCount { get; set; }

        public long? RepliedToReviewId { get; set; }

        public virtual EntityReview RepliedToReview { get; set; }

        public virtual ICollection<EntityReview> Replies { get; set; }
    }
}