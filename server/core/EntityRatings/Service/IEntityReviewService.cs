using Core;
using Core.Database;
using Core.Exceptions;
using Core.ObjectMapping;
using EntityReviews.Dtos;
using EntityReviews.Models;
using EntityReviews.Repositories;
using System;
using System.Collections.Generic;

namespace EntityReviews.Service
{
    public interface IEntityReviewService : IBaseService<EntityReview, EntityReviewModel>, IDependency
    {
        IEnumerable<EntityReviewModel> GetAllReviewsForEntity<T>(long reviewedEntityId);

        IEnumerable<EntityReviewModel> GetAllReviewsForEntity<T>();

        void ReplyToReview(long reviewId, EntityReviewModel replyModel);

        EntityReviewModel GetReview<T>(long reviewId);

        EntityReviewModel GetReview<T>(long reviewedEntityId, long connectedEntityId);
        IEnumerable<EntityReviewModel> GetReviews<T>(long reviewedEntityId, long connectedEntityId);

        void SaveReview<T>(EntityReviewModel model);

        void UpdateReview<T>(long id, long reviewerId, EntityReviewModel model);

        void ApproveReview(long reviewId, string approveBy);

        void LikeReview(long reviewId);

        void DislikeReview(long reviewId);
    }

    public class EntityReviewService : BaseService<EntityReview, EntityReviewModel>, IEntityReviewService
    {
        private static readonly object LockObject = new object();

        public EntityReviewService(IMappingService mappingService, IRepository repository)
            : base(mappingService, repository)
        {
        }

        public IEnumerable<EntityReviewModel> GetAllReviewsForEntity<T>()
        {
            var entityReviews = Fetch(x => x.TargetEntityObject == typeof(T).FullName);

            return ToDtos(entityReviews);
        }

        public void ReplyToReview(long reviewId, EntityReviewModel replyModel)
        {
            var entity = MappingService.Map<EntityReview>(replyModel);

            entity.RepliedToReviewId = reviewId;
            Repository.Insert(entity);
            Repository.SaveChanges();
        }


        public IEnumerable<EntityReviewModel> GetAllReviewsForEntity<T>(long reviewedEntityId)
        {
            var entityReviews = Fetch(x => x.TargetEntityId == reviewedEntityId && x.TargetEntityObject == typeof(T).FullName);

            return ToDtos(entityReviews);
        }

        public EntityReviewModel GetReview<T>(long reviewId)
        {
            var entityReview = First(x => x.Id == reviewId && x.TargetEntityObject == typeof(T).FullName);

            if (entityReview == null)
                throw new BaseNotFoundException<EntityReviewModel>();

            return ToDto(entityReview);
        }

        public EntityReviewModel GetReview<T>(long reviewedEntityId, long connectedEntityId)
        {
            var entityReview =
                First(
                    x =>
                    x.TargetEntityId == reviewedEntityId && x.ConnectedEntityId == connectedEntityId &&
                    x.TargetEntityObject == typeof(T).FullName);

            if (entityReview == null)
                throw new BaseNotFoundException<EntityReviewModel>();

            return ToDto(entityReview);
        }

        public IEnumerable<EntityReviewModel> GetReviews<T>(long reviewedEntityId, long connectedEntityId)
        {
            var entityReviews =
                Fetch(
                      x =>
                          x.TargetEntityId     == reviewedEntityId && x.ConnectedEntityId == connectedEntityId &&
                          x.TargetEntityObject == typeof(T).FullName);

            return ToDtos(entityReviews);
        }

        public void SaveReview<T>(EntityReviewModel model)
        {
            if (model.ReviewerUserId == 0)
                throw new InvalidOperationException("Reviewer has to be specified");

            var entity = MappingService.Map<EntityReview>(model);

            entity.TargetEntityObject = typeof(T).FullName;

            entity.CreatedDate = DateTimeOffset.Now;
            entity.ModifiedDate = DateTimeOffset.Now;

            if (entity.IsApproved)
            {
                entity.ApprovedDate = DateTimeOffset.Now;
                entity.ApprovedBy = "";
            }
            Repository.Insert(entity);
            Repository.SaveChanges();
        }

        public void UpdateReview<T>(long id, long reviewerId, EntityReviewModel model)
        {
            var entity = GetEntity(id);

            if (entity == null || entity.TargetEntityObject != typeof(T).FullName)
                throw new BaseNotFoundException<EntityReview>();

            if (entity.ReviewerUserId != reviewerId)
                throw new UnauthorizedAccessException("You cannot modified review submitted by another user");

            entity.Rating = model.Rating;
            entity.ReviewText = model.ReviewText;
            entity.Title = model.Title;
            entity.ModifiedDate = DateTimeOffset.Now;

            Repository.Update(entity);
            Repository.SaveChanges();
        }

        public void ApproveReview(long reviewId, string approveBy)
        {
            var entity = GetEntity(reviewId);

            entity.IsApproved = true;
            entity.ApprovedDate = DateTimeOffset.Now;
            entity.ApprovedBy = approveBy;
            Repository.Update(entity);
            Repository.SaveChanges();
        }

        public void LikeReview(long reviewId)
        {
            lock (LockObject)
            {
                var entity = GetEntity(reviewId);

                entity.LikesCount++;
                Repository.Update(entity);
                Repository.SaveChanges();
            }
        }

        public void DislikeReview(long reviewId)
        {
            lock (LockObject)
            {
                var entity = GetEntity(reviewId);

                entity.DislikesCount++;
                Repository.Update(entity);
                Repository.SaveChanges();
            }
        }
    }
}