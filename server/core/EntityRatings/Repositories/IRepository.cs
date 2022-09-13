using Core;
using Core.Database.Repositories;
using EntityReviews.DbContexts;

namespace EntityReviews.Repositories
{
    public interface IRepository : IBaseRepository, IDependency
    {
    }

    public class EntityRatingEfRepository : Repository, IRepository
    {
        public EntityRatingEfRepository(EntityReviewContext dbContext) : base(dbContext)
        {
        }
    }
}