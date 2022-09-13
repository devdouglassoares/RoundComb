using Newtonsoft.Json;

namespace Subscription.Core.Entities
{
    public class SubscriptionPlanAccessEntity
    {
        public long SubscriptionPlanId { get; set; }

        public long AccessEntityId { get; set; }

        [JsonIgnore]
        public virtual SubscriptionPlan SubscriptionPlan { get; set; }
    }
}