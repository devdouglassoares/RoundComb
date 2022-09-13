using System;
using System.Collections.Generic;

namespace EntityReviews.Dtos
{
    public class EntityReviewModel
    {
        public long Id { get; set; }

        public long? ReviewerUserId { get; set; }

        public dynamic ReviewerUser { get; set; }

        public long TargetEntityId { get; set; }

        public dynamic TargetEntity { get; set; }

        public long? ConnectedEntityId { get; set; }

        public dynamic ConnectedEntity { get; set; }

        public string Title { get; set; }

        public string ReviewText { get; set; }

        public int Rating { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }

        public bool IsApproved { get; set; }

        public DateTimeOffset? ApprovedDate { get; set; }

        public string ApprovedBy { get; set; }

        public int LikesCount { get; set; }

        public int DislikesCount { get; set; }

        public IList<EntityReviewModel> Replies { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}