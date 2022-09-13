namespace Core.DynamicProperties.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Support_Multiple_TargetEntityTypes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DYNAMIC_PROP_DynamicPropertyEntityTypeMapping",
                c => new
                    {
                        DynamicPropertyId = c.Long(nullable: false),
                        TargetEntityType = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.DynamicPropertyId, t.TargetEntityType })
                .ForeignKey("dbo.DYNAMIC_PROP_DynamicProperty", t => t.DynamicPropertyId, cascadeDelete: true)
                .Index(t => t.DynamicPropertyId);
            
            DropColumn("dbo.DYNAMIC_PROP_DynamicProperty", "TargetEntityType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DYNAMIC_PROP_DynamicProperty", "TargetEntityType", c => c.String());
            DropForeignKey("dbo.DYNAMIC_PROP_DynamicPropertyEntityTypeMapping", "DynamicPropertyId", "dbo.DYNAMIC_PROP_DynamicProperty");
            DropIndex("dbo.DYNAMIC_PROP_DynamicPropertyEntityTypeMapping", new[] { "DynamicPropertyId" });
            DropTable("dbo.DYNAMIC_PROP_DynamicPropertyEntityTypeMapping");
        }
    }
}
