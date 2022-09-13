namespace Core.DynamicProperties.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_PrimaryKey_To_DynamicPropertyValue : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.DYNAMIC_PROP_DynamicPropertyValue");
            AddColumn("dbo.DYNAMIC_PROP_DynamicPropertyValue", "Id", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.DYNAMIC_PROP_DynamicPropertyValue", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.DYNAMIC_PROP_DynamicPropertyValue");
            DropColumn("dbo.DYNAMIC_PROP_DynamicPropertyValue", "Id");
            AddPrimaryKey("dbo.DYNAMIC_PROP_DynamicPropertyValue", new[] { "PropertyId", "ExternalEntityId" });
        }
    }
}
