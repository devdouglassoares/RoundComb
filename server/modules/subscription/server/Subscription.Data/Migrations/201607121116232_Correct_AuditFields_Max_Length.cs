namespace Subscription.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Correct_AuditFields_Max_Length : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SubscriptionPlan", "CreatedBy", c => c.String());
            AlterColumn("dbo.SubscriptionPlan", "ModifiedBy", c => c.String());
            AlterColumn("dbo.SubscriptionInvoice", "CreatedBy", c => c.String());
            AlterColumn("dbo.SubscriptionInvoice", "ModifiedBy", c => c.String());
            AlterColumn("dbo.UserSubscription", "CreatedBy", c => c.String());
            AlterColumn("dbo.UserSubscription", "ModifiedBy", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserSubscription", "ModifiedBy", c => c.String(maxLength: 30));
            AlterColumn("dbo.UserSubscription", "CreatedBy", c => c.String(maxLength: 30));
            AlterColumn("dbo.SubscriptionInvoice", "ModifiedBy", c => c.String(maxLength: 30));
            AlterColumn("dbo.SubscriptionInvoice", "CreatedBy", c => c.String(maxLength: 30));
            AlterColumn("dbo.SubscriptionPlan", "ModifiedBy", c => c.String(maxLength: 30));
            AlterColumn("dbo.SubscriptionPlan", "CreatedBy", c => c.String(maxLength: 30));
        }
    }
}
