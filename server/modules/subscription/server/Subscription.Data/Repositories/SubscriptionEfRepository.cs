using Core.Database.Repositories;
using Subscription.Core.Repositories;
using Subscription.Data.Db;

namespace Subscription.Data.Repositories
{
    public class SubscriptionEfRepository: Repository, IRepository
    {
        public SubscriptionEfRepository(SubscriptionContext dbContext) : base(dbContext)
        {
        }
    }
}