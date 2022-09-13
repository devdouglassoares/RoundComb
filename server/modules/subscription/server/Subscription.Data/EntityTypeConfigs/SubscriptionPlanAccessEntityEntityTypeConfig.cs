using System.Data.Entity.ModelConfiguration;
using Subscription.Core.Entities;

namespace Subscription.Data.EntityTypeConfigs
{
    public class SubscriptionPlanAccessEntityEntityTypeConfig : EntityTypeConfiguration<SubscriptionPlanAccessEntity>
    {
        public SubscriptionPlanAccessEntityEntityTypeConfig()
        {
            HasKey(accessEntity => new { accessEntity.AccessEntityId, accessEntity.SubscriptionPlanId });
        }
    }

    public class InvoiceEntityTypeConfig : EntityTypeConfiguration<SubscriptionInvoice>
    {
        public InvoiceEntityTypeConfig()
        {
            HasKey(subscriptionInvoice => subscriptionInvoice.Id);
        }
    }
}