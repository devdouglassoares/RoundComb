namespace Core.DynamicProperties.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_RangeFilter_Support : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DYNAMIC_PROP_DynamicProperty", "RangeSearchable", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DYNAMIC_PROP_DynamicProperty", "RangeSearchable");
        }
    }
}
