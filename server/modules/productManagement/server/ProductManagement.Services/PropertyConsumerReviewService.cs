using EntityReviews.Dtos;
using EntityReviews.Service;
using ProductManagement.Core.Services;
using Membership.Core;
using Membership.Core.Contracts;
using Membership.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ProductManagement.Services
{
    public class PropertyConsumerReviewService : IPropertyConsumerReviewService
	{
		private readonly IEntityReviewService _entityReviewService;
		private readonly IMembership _membership;
		private readonly IUserService _userService;
		private readonly IPropertyService _propertyService;

		public PropertyConsumerReviewService(IEntityReviewService entityReviewService, IMembership membership, IUserService userService, IPropertyService propertyService)
		{
			_entityReviewService = entityReviewService;
			_membership = membership;
			_userService = userService;
			_propertyService = propertyService;
		}

		public EntityReviewModel GetReview(long reviewId)
		{
			var entityReviewModel = _entityReviewService.GetReview<User>(reviewId);

			MapConnectedEntityData(entityReviewModel);

			return entityReviewModel;
		}

		public EntityReviewModel GetReview(long entityId, long connectedEntityId)
		{
			var entityReviewModel = _entityReviewService.GetReview<User>(entityId, connectedEntityId);

			MapConnectedEntityData(entityReviewModel);

			return entityReviewModel;
		}

	    public IEnumerable<EntityReviewModel> GetReviews(long entityId, long connectedEntityId)
	    {
	        var dtos = _entityReviewService.GetReviews<User>(entityId, connectedEntityId);

	        foreach (var entityReviewModel in dtos)
	        {
	            MapConnectedEntityData(entityReviewModel);
	        }

            return dtos;
	    }

        public void UpdateReview(long reviewId, EntityReviewModel model)
		{
			_entityReviewService.UpdateReview<User>(reviewId, _membership.UserId, model);
		}

		public void SaveReview(EntityReviewModel model)
		{
			model.ReviewerUserId = _membership.UserId;
			_entityReviewService.SaveReview<User>(model);
		}

		public IEnumerable<EntityReviewModel> GetConsumerReviews(long consumerId)
		{
			var dtos = _entityReviewService.GetAllReviewsForEntity<User>(consumerId)
										   .Where(x => x.ConnectedEntityId.HasValue && x.ReviewerUserId.HasValue)
										   .ToArray();

			foreach (var entityReviewModel in dtos)
			{
				MapConnectedEntityData(entityReviewModel);
			}

			return dtos;
		}

		private void MapConnectedEntityData(EntityReviewModel entityReviewModel)
		{
			if (entityReviewModel.ReviewerUserId != null)
			{
				var reviewerUser = _userService.GetUserPersonalInformation(entityReviewModel.ReviewerUserId.Value);
				if (reviewerUser != null)
				{
					entityReviewModel.ReviewerUser = reviewerUser;
				}
			}

			var userInfo = _userService.GetUserPersonalInformation(entityReviewModel.TargetEntityId);
			if (userInfo != null)
			{
				entityReviewModel.TargetEntity = userInfo;
			}


			try
			{
				var property = _propertyService.GetDto(entityReviewModel.ConnectedEntityId);
				entityReviewModel.ConnectedEntity = property;
			}
			catch (System.Exception)
			{
				// do nothing, just ignore the error
				entityReviewModel.ConnectedEntity = null;
			}
		}
	}
}