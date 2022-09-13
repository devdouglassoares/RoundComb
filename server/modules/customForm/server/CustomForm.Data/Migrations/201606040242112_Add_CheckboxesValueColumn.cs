namespace CustomForm.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_CheckboxesValueColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FormFieldValue", "CheckboxValues", c => c.String());
            DropColumn("dbo.FormFieldValue", "UploadedPath");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FormFieldValue", "UploadedPath", c => c.String());
            DropColumn("dbo.FormFieldValue", "CheckboxValues");
        }
    }
}
