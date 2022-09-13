namespace Core.DynamicProperties.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_BaseEntity_To_DynamicProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DYNAMIC_PROP_DynamicProperty", "CreatedDate", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.DYNAMIC_PROP_DynamicProperty", "ModifiedDate", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.DYNAMIC_PROP_DynamicProperty", "CreatedBy", c => c.String());
            AddColumn("dbo.DYNAMIC_PROP_DynamicProperty", "ModifiedBy", c => c.String());
            AddColumn("dbo.DYNAMIC_PROP_DynamicPropertyValue", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.DYNAMIC_PROP_DynamicPropertyValue", "CreatedDate", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.DYNAMIC_PROP_DynamicPropertyValue", "ModifiedDate", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.DYNAMIC_PROP_DynamicPropertyValue", "CreatedBy", c => c.String());
            AddColumn("dbo.DYNAMIC_PROP_DynamicPropertyValue", "ModifiedBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DYNAMIC_PROP_DynamicPropertyValue", "ModifiedBy");
            DropColumn("dbo.DYNAMIC_PROP_DynamicPropertyValue", "CreatedBy");
            DropColumn("dbo.DYNAMIC_PROP_DynamicPropertyValue", "ModifiedDate");
            DropColumn("dbo.DYNAMIC_PROP_DynamicPropertyValue", "CreatedDate");
            DropColumn("dbo.DYNAMIC_PROP_DynamicPropertyValue", "IsDeleted");
            DropColumn("dbo.DYNAMIC_PROP_DynamicProperty", "ModifiedBy");
            DropColumn("dbo.DYNAMIC_PROP_DynamicProperty", "CreatedBy");
            DropColumn("dbo.DYNAMIC_PROP_DynamicProperty", "ModifiedDate");
            DropColumn("dbo.DYNAMIC_PROP_DynamicProperty", "CreatedDate");
        }
    }
}
