namespace Core.DynamicProperties.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_UploadedPaths_Support_For_Dynamic_Upload_Control : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DYNAMIC_PROP_DynamicProperty", "UploadEndpointUrl", c => c.String());
            AddColumn("dbo.DYNAMIC_PROP_DynamicProperty", "MultipleUpload", c => c.Boolean(nullable: false));
            AddColumn("dbo.DYNAMIC_PROP_DynamicPropertyValue", "UploadedPaths", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DYNAMIC_PROP_DynamicPropertyValue", "UploadedPaths");
            DropColumn("dbo.DYNAMIC_PROP_DynamicProperty", "MultipleUpload");
            DropColumn("dbo.DYNAMIC_PROP_DynamicProperty", "UploadEndpointUrl");
        }
    }
}
