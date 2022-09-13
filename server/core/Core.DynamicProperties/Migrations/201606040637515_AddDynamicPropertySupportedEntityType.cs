namespace Core.DynamicProperties.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDynamicPropertySupportedEntityType : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.DYNAMIC_PROP_DynamicPropertyEntityTypeMapping");
            CreateTable(
                "dbo.DYNAMIC_PROP_DynamicPropertySupportedEntityType",
                c => new
                    {
                        EntityTypeFullName = c.String(nullable: false, maxLength: 450),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.EntityTypeFullName);
            
            AlterColumn("dbo.DYNAMIC_PROP_DynamicPropertyEntityTypeMapping", "TargetEntityType", c => c.String(nullable: false, maxLength: 440));
            AddPrimaryKey("dbo.DYNAMIC_PROP_DynamicPropertyEntityTypeMapping", new[] { "DynamicPropertyId", "TargetEntityType" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.DYNAMIC_PROP_DynamicPropertyEntityTypeMapping");
            AlterColumn("dbo.DYNAMIC_PROP_DynamicPropertyEntityTypeMapping", "TargetEntityType", c => c.String(nullable: false, maxLength: 128));
            DropTable("dbo.DYNAMIC_PROP_DynamicPropertySupportedEntityType");
            AddPrimaryKey("dbo.DYNAMIC_PROP_DynamicPropertyEntityTypeMapping", new[] { "DynamicPropertyId", "TargetEntityType" });
        }
    }
}
