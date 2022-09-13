using Core;
using Membership.Core.Entities;
using Subscription.Core.Entities;
using System;

namespace Subscription.Service.Infrastructure.Facades
{
    public interface ISubscriptionsFacade : IDependency
    {
        UserSubscription SubscribeUserAsync(User user, string planId, string creditCardToken = null);

        int CalculateProRata(long planId);

        DateTime? GetStartNextMonth();
    }
}