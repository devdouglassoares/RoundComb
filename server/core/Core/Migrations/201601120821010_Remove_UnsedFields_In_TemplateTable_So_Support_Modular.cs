namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Remove_UnsedFields_In_TemplateTable_So_Support_Modular : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TEMPLATE_TemplateModel", "Fields", c => c.String());
            AddColumn("dbo.TEMPLATE_TemplateModel", "FromAssemblyName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TEMPLATE_TemplateModel", "FromAssemblyName");
            DropColumn("dbo.TEMPLATE_TemplateModel", "Fields");
        }
    }
}
