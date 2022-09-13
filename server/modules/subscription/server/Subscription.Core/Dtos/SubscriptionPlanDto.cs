using Subscription.Core.Entities;
using System.Collections.Generic;

namespace Subscription.Core.Dtos
{
    public class SubscriptionPlanDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public bool IsTrialSubscription { get; set; }

        public double Price { get; set; }

        public string Currency { get; set; }

        public SubscriptionInterval Interval { get; set; }

        public int PlanLevel { get; set; }

        public int TrialPeriodInDays { get; set; }

        public bool Disabled { get; set; }

        public long? AssignRoleId { get; set; }

        public IDictionary<string, string> Properties { get; set; }

        public virtual ICollection<long> AccessEntityIds { get; set; }
    }
}