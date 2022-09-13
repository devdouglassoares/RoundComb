namespace Core.DynamicProperties.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Required_Property_To_DynamicProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DYNAMIC_PROP_DynamicProperty", "IsRequired", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DYNAMIC_PROP_DynamicProperty", "IsRequired");
        }
    }
}
