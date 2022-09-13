namespace CustomForm.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Correct_AuditFields_Max_Length : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FormConfiguration", "CreatedBy", c => c.String());
            AlterColumn("dbo.FormConfiguration", "ModifiedBy", c => c.String());
            AlterColumn("dbo.FormField", "CreatedBy", c => c.String());
            AlterColumn("dbo.FormField", "ModifiedBy", c => c.String());
            AlterColumn("dbo.FormInstance", "CreatedBy", c => c.String());
            AlterColumn("dbo.FormInstance", "ModifiedBy", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FormInstance", "ModifiedBy", c => c.String(maxLength: 30));
            AlterColumn("dbo.FormInstance", "CreatedBy", c => c.String(maxLength: 30));
            AlterColumn("dbo.FormField", "ModifiedBy", c => c.String(maxLength: 30));
            AlterColumn("dbo.FormField", "CreatedBy", c => c.String(maxLength: 30));
            AlterColumn("dbo.FormConfiguration", "ModifiedBy", c => c.String(maxLength: 30));
            AlterColumn("dbo.FormConfiguration", "CreatedBy", c => c.String(maxLength: 30));
        }
    }
}
