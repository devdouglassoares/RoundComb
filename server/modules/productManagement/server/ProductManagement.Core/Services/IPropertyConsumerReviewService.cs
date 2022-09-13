using Core;
using EntityReviews.Dtos;
using System.Collections.Generic;

namespace ProductManagement.Core.Services
{
    public interface IPropertyConsumerReviewService : IDependency
    {
        EntityReviewModel GetReview(long reviewId);

        EntityReviewModel GetReview(long entityId, long connectedEntityId);
        IEnumerable<EntityReviewModel> GetReviews(long entityId, long connectedEntityId);

        void UpdateReview(long reviewId, EntityReviewModel model);

        void SaveReview(EntityReviewModel model);

        IEnumerable<EntityReviewModel> GetConsumerReviews(long consumerId);
    }
}