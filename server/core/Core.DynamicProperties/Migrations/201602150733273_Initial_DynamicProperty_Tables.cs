namespace Core.DynamicProperties.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial_DynamicProperty_Tables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DYNAMIC_PROP_DynamicProperty",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DisplayName = c.String(),
                        PropertyName = c.String(),
                        Searchable = c.Boolean(nullable: false),
                        PropertyType = c.String(),
                        AvailableToAllEntities = c.Boolean(nullable: false),
                        TargetEntityType = c.String(),
                        AvailableOptions = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DYNAMIC_PROP_DynamicPropertyValue",
                c => new
                    {
                        PropertyId = c.Long(nullable: false),
                        ExternalEntityId = c.Long(nullable: false),
                        TargetEntityType = c.String(),
                        PropertyValue = c.String(),
                    })
                .PrimaryKey(t => new { t.PropertyId, t.ExternalEntityId })
                .ForeignKey("dbo.DYNAMIC_PROP_DynamicProperty", t => t.PropertyId, cascadeDelete: true)
                .Index(t => t.PropertyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DYNAMIC_PROP_DynamicPropertyValue", "PropertyId", "dbo.DYNAMIC_PROP_DynamicProperty");
            DropIndex("dbo.DYNAMIC_PROP_DynamicPropertyValue", new[] { "PropertyId" });
            DropTable("dbo.DYNAMIC_PROP_DynamicPropertyValue");
            DropTable("dbo.DYNAMIC_PROP_DynamicProperty");
        }
    }
}
