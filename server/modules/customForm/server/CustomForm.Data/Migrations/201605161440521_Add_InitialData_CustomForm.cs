namespace CustomForm.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_InitialData_CustomForm : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FormConfiguration",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FormName = c.String(),
                        OwnerId = c.Long(),
                        IsSystemConfig = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        CreatedBy = c.String(maxLength: 30),
                        ModifiedBy = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FormFieldFormConfiguration",
                c => new
                    {
                        FormFieldId = c.Long(nullable: false),
                        FormConfigurationId = c.Long(nullable: false),
                        DisplaySectionCode = c.String(),
                        DisplayOrder = c.Int(nullable: false),
                        Display = c.Boolean(nullable: false),
                        IsRequired = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.FormFieldId, t.FormConfigurationId })
                .ForeignKey("dbo.FormConfiguration", t => t.FormConfigurationId, cascadeDelete: true)
                .ForeignKey("dbo.FormField", t => t.FormFieldId, cascadeDelete: true)
                .Index(t => t.FormFieldId)
                .Index(t => t.FormConfigurationId);
            
            CreateTable(
                "dbo.FormField",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FieldName = c.String(),
                        DisplayName = c.String(),
                        LocalizationKey = c.String(),
                        FieldType = c.String(),
                        PredefinedValues = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        CreatedBy = c.String(maxLength: 30),
                        ModifiedBy = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FormFieldValue",
                c => new
                    {
                        FieldId = c.Long(nullable: false),
                        FormInstanceId = c.Long(nullable: false),
                        StringValue = c.String(),
                        IntValue = c.Int(),
                        DecimalValue = c.Decimal(precision: 18, scale: 2),
                        DateTimeValue = c.DateTimeOffset(precision: 7),
                        BooleanValue = c.Boolean(),
                        UploadedPath = c.String(),
                    })
                .PrimaryKey(t => new { t.FieldId, t.FormInstanceId })
                .ForeignKey("dbo.FormField", t => t.FieldId, cascadeDelete: true)
                .ForeignKey("dbo.FormInstance", t => t.FormInstanceId, cascadeDelete: true)
                .Index(t => t.FieldId)
                .Index(t => t.FormInstanceId);
            
            CreateTable(
                "dbo.FormInstance",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        FormConfigurationId = c.Long(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        CreatedBy = c.String(maxLength: 30),
                        ModifiedBy = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FormConfiguration", t => t.FormConfigurationId, cascadeDelete: true)
                .Index(t => t.FormConfigurationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FormInstance", "FormConfigurationId", "dbo.FormConfiguration");
            DropForeignKey("dbo.FormFieldValue", "FormInstanceId", "dbo.FormInstance");
            DropForeignKey("dbo.FormFieldValue", "FieldId", "dbo.FormField");
            DropForeignKey("dbo.FormFieldFormConfiguration", "FormFieldId", "dbo.FormField");
            DropForeignKey("dbo.FormFieldFormConfiguration", "FormConfigurationId", "dbo.FormConfiguration");
            DropIndex("dbo.FormInstance", new[] { "FormConfigurationId" });
            DropIndex("dbo.FormFieldValue", new[] { "FormInstanceId" });
            DropIndex("dbo.FormFieldValue", new[] { "FieldId" });
            DropIndex("dbo.FormFieldFormConfiguration", new[] { "FormConfigurationId" });
            DropIndex("dbo.FormFieldFormConfiguration", new[] { "FormFieldId" });
            DropTable("dbo.FormInstance");
            DropTable("dbo.FormFieldValue");
            DropTable("dbo.FormField");
            DropTable("dbo.FormFieldFormConfiguration");
            DropTable("dbo.FormConfiguration");
        }
    }
}
