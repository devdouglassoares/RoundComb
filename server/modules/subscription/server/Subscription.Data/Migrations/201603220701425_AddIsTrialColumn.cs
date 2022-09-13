namespace Subscription.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsTrialColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SubscriptionPlan", "IsTrialSubscription", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SubscriptionPlan", "IsTrialSubscription");
        }
    }
}
