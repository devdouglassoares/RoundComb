using Core.ObjectMapping;
using EntityReviews.Dtos;
using EntityReviews.Models;

namespace EntityReviews.ModelMapping
{
    public class EntityReviewModelMappingRegistrar : IObjectMappingRegistration
    {
        public void ConfigureMapping(IMappingService map)
        {
            map.ConfigureMapping<EntityReview, EntityReviewModel>();

            map.ConfigureMapping<EntityReviewModel, EntityReview>();
        }
    }
}