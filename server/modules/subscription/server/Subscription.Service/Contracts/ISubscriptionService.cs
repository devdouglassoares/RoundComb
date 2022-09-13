using System.Collections.Generic;
using IDependency = Core.IDependency;

namespace Subscription.Service.Contracts
{
    public interface ISubscriptionService : IDependency
    {
        IEnumerable<Core.Entities.UserSubscription> GetCurrentUserSubscriptions();

        Core.Entities.UserSubscription GetUserActiveSubscription(long userId);

        IEnumerable<long> GetSubscribedUserIdsByStartDateDescending();
    }
}