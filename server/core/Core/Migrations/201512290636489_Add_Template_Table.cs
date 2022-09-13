namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Template_Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TEMPLATE_TemplateModel",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TemplateType = c.String(),
                        TemplateTitle = c.String(),
                        TemplateContent = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        CreatedBy = c.String(maxLength: 30),
                        ModifiedBy = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TEMPLATE_TemplateModel");
        }
    }
}
