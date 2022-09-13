namespace Subscription.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveSubscriptionPropertyTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SubscriptionPlanProperty", "SubscriptionPlanId", "dbo.SubscriptionPlan");
            DropIndex("dbo.SubscriptionPlanProperty", "IX_SubscriptionPlan_Id");
            AddColumn("dbo.SubscriptionPlan", "Properties", c => c.String());
            DropTable("dbo.SubscriptionPlanProperty");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SubscriptionPlanProperty",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Key = c.String(),
                        Value = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        CreatedBy = c.String(maxLength: 30),
                        ModifiedBy = c.String(maxLength: 30),
                        SubscriptionPlanId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.SubscriptionPlan", "Properties");
            CreateIndex("dbo.SubscriptionPlanProperty", "SubscriptionPlanId", name: "IX_SubscriptionPlan_Id");
            AddForeignKey("dbo.SubscriptionPlanProperty", "SubscriptionPlanId", "dbo.SubscriptionPlan", "Id", cascadeDelete: true);
        }
    }
}
