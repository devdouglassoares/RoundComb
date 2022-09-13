namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Correct_AuditFields_Max_Length : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TEMPLATE_TemplateModel", "CreatedBy", c => c.String());
            AlterColumn("dbo.TEMPLATE_TemplateModel", "ModifiedBy", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TEMPLATE_TemplateModel", "ModifiedBy", c => c.String(maxLength: 30));
            AlterColumn("dbo.TEMPLATE_TemplateModel", "CreatedBy", c => c.String(maxLength: 30));
        }
    }
}
