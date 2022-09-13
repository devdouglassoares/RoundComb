namespace Core.DynamicProperties.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migrate_DynamicProperty_Support_More_Concrete_Type : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DYNAMIC_PROP_DynamicPropertyValue", "StringValue", c => c.String());
            AddColumn("dbo.DYNAMIC_PROP_DynamicPropertyValue", "IntValue", c => c.Int());
            AddColumn("dbo.DYNAMIC_PROP_DynamicPropertyValue", "DecimalValue", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.DYNAMIC_PROP_DynamicPropertyValue", "DateTimeValue", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.DYNAMIC_PROP_DynamicPropertyValue", "BooleanValue", c => c.Boolean());
            DropColumn("dbo.DYNAMIC_PROP_DynamicPropertyValue", "PropertyValue");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DYNAMIC_PROP_DynamicPropertyValue", "PropertyValue", c => c.String());
            DropColumn("dbo.DYNAMIC_PROP_DynamicPropertyValue", "BooleanValue");
            DropColumn("dbo.DYNAMIC_PROP_DynamicPropertyValue", "DateTimeValue");
            DropColumn("dbo.DYNAMIC_PROP_DynamicPropertyValue", "DecimalValue");
            DropColumn("dbo.DYNAMIC_PROP_DynamicPropertyValue", "IntValue");
            DropColumn("dbo.DYNAMIC_PROP_DynamicPropertyValue", "StringValue");
        }
    }
}
