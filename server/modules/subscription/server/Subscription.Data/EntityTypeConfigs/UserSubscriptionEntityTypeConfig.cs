using System.Data.Entity.ModelConfiguration;
using Subscription.Core.Entities;

namespace Subscription.Data.EntityTypeConfigs
{
    public class UserSubscriptionEntityTypeConfig : EntityTypeConfiguration<UserSubscription>
    {
        public UserSubscriptionEntityTypeConfig()
        {
            HasKey(userSubscription => userSubscription.Id);

            HasRequired(userSubscription => userSubscription.SubscriptionPlan);
        }
    }
}