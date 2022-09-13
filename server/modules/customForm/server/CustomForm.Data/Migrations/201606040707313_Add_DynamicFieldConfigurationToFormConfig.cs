namespace CustomForm.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_DynamicFieldConfigurationToFormConfig : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FormDynamicPropertyConfig",
                c => new
                    {
                        FormConfigurationId = c.Long(nullable: false),
                        DynamicPropertyId = c.Long(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.FormConfigurationId, t.DynamicPropertyId })
                .ForeignKey("dbo.FormConfiguration", t => t.FormConfigurationId, cascadeDelete: true)
                .Index(t => t.FormConfigurationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FormDynamicPropertyConfig", "FormConfigurationId", "dbo.FormConfiguration");
            DropIndex("dbo.FormDynamicPropertyConfig", new[] { "FormConfigurationId" });
            DropTable("dbo.FormDynamicPropertyConfig");
        }
    }
}
