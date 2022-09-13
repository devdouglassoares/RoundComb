namespace Subscription.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSubscriptionTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SubscriptionPlanAccessEntity",
                c => new
                    {
                        AccessEntityId = c.Long(nullable: false),
                        SubscriptionPlanId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.AccessEntityId, t.SubscriptionPlanId })
                .ForeignKey("dbo.SubscriptionPlan", t => t.SubscriptionPlanId, cascadeDelete: true)
                .Index(t => t.SubscriptionPlanId);
            
            CreateTable(
                "dbo.SubscriptionPlan",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Price = c.Double(nullable: false),
                        Currency = c.String(),
                        Interval = c.Int(nullable: false),
                        TrialPeriodInDays = c.Int(nullable: false),
                        Disabled = c.Boolean(nullable: false),
                        PlanLevel = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        CreatedBy = c.String(maxLength: 30),
                        ModifiedBy = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SubscriptionPlan", t => t.SubscriptionPlanId, cascadeDelete: true)
                .Index(t => t.SubscriptionPlanId, name: "IX_SubscriptionPlan_Id");
            
            CreateTable(
                "dbo.SubscriptionInvoice",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ExternalId = c.String(maxLength: 50),
                        ExternalCustomerId = c.String(maxLength: 50),
                        Date = c.DateTime(precision: 7, storeType: "datetime2"),
                        PeriodStart = c.DateTime(precision: 7, storeType: "datetime2"),
                        PeriodEnd = c.DateTime(precision: 7, storeType: "datetime2"),
                        Subtotal = c.Int(),
                        Total = c.Int(),
                        Attempted = c.Boolean(),
                        Closed = c.Boolean(),
                        Paid = c.Boolean(),
                        AttemptCount = c.Int(),
                        AmountDue = c.Int(),
                        StartingBalance = c.Int(),
                        EndingBalance = c.Int(),
                        NextPaymentAttempt = c.DateTime(precision: 7, storeType: "datetime2"),
                        ApplicationFee = c.Int(),
                        Tax = c.Int(),
                        TaxPercent = c.Decimal(precision: 18, scale: 2),
                        Currency = c.String(),
                        Description = c.String(),
                        StatementDescriptor = c.String(),
                        ReceiptNumber = c.String(),
                        Forgiven = c.Boolean(nullable: false),
                        CustomerId = c.Long(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        CreatedBy = c.String(maxLength: 30),
                        ModifiedBy = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserSubscription",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Start = c.DateTime(precision: 7, storeType: "datetime2"),
                        End = c.DateTime(precision: 7, storeType: "datetime2"),
                        TrialStart = c.DateTime(precision: 7, storeType: "datetime2"),
                        TrialEnd = c.DateTime(precision: 7, storeType: "datetime2"),
                        SubscriptionPlanId = c.Long(nullable: false),
                        UserId = c.Long(nullable: false),
                        ExternalId = c.String(maxLength: 50),
                        Status = c.String(),
                        TaxPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ReasonToCancel = c.String(),
                        ExternalCustomerId = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        CreatedBy = c.String(maxLength: 30),
                        ModifiedBy = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SubscriptionPlan", t => t.SubscriptionPlanId, cascadeDelete: true)
                .Index(t => t.SubscriptionPlanId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserSubscription", "SubscriptionPlanId", "dbo.SubscriptionPlan");
            DropForeignKey("dbo.SubscriptionPlanProperty", "SubscriptionPlanId", "dbo.SubscriptionPlan");
            DropForeignKey("dbo.SubscriptionPlanAccessEntity", "SubscriptionPlanId", "dbo.SubscriptionPlan");
            DropIndex("dbo.UserSubscription", new[] { "SubscriptionPlanId" });
            DropIndex("dbo.SubscriptionPlanProperty", "IX_SubscriptionPlan_Id");
            DropIndex("dbo.SubscriptionPlanAccessEntity", new[] { "SubscriptionPlanId" });
            DropTable("dbo.UserSubscription");
            DropTable("dbo.SubscriptionInvoice");
            DropTable("dbo.SubscriptionPlanProperty");
            DropTable("dbo.SubscriptionPlan");
            DropTable("dbo.SubscriptionPlanAccessEntity");
        }
    }
}
