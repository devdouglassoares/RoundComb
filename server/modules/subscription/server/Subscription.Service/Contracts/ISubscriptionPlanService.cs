using Core;
using Core.Database;
using Subscription.Core.Dtos;
using Subscription.Core.Entities;

namespace Subscription.Service.Contracts
{
    public interface ISubscriptionPlanService : IBaseService<SubscriptionPlan, SubscriptionPlanDto>, IDependency
    {
    }
}