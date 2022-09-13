namespace Core.DynamicProperties.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Checkbox_Values_Support_To_DynamicPropertyValue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DYNAMIC_PROP_DynamicPropertyValue", "CheckboxValues", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DYNAMIC_PROP_DynamicPropertyValue", "CheckboxValues");
        }
    }
}
