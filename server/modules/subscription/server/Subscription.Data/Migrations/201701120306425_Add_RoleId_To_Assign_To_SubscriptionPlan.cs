namespace Subscription.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_RoleId_To_Assign_To_SubscriptionPlan : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SubscriptionPlan", "AssignRoleId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SubscriptionPlan", "AssignRoleId");
        }
    }
}
