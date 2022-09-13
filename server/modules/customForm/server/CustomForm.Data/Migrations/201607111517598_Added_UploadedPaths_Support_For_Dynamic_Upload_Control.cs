namespace CustomForm.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_UploadedPaths_Support_For_Dynamic_Upload_Control : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FormField", "UploadEndpointUrl", c => c.String());
            AddColumn("dbo.FormField", "MultipleUpload", c => c.Boolean(nullable: false));
            AddColumn("dbo.FormFieldValue", "UploadedPaths", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FormFieldValue", "UploadedPaths");
            DropColumn("dbo.FormField", "MultipleUpload");
            DropColumn("dbo.FormField", "UploadEndpointUrl");
        }
    }
}
