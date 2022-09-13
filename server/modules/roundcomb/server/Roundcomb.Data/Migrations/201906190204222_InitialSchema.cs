namespace Roundcomb.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSchema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PROD_PropertyApplicationFormDocumentConfig",
                c => new
                    {
                        PropertyId = c.Long(nullable: false),
                        Id = c.Long(nullable: false),
                        FileName = c.String(),
                        FileUrl = c.String(),
                        IsExternalSite = c.Boolean(nullable: false),
                        ResultUrl = c.String(),
                        DownloadTime = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => new { t.PropertyId, t.Id });
            
            CreateTable(
                "dbo.PROD_PropertyApplicationFormInstance",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UploadedApplicationFileName = c.String(),
                        UploadedApplicationFileUrl = c.String(),
                        IsExternalSite = c.Boolean(nullable: false),
                        ResultUrl = c.String(),
                        FormInstanceId = c.Long(nullable: false),
                        UserId = c.Long(nullable: false),
                        PropertyId = c.Long(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        ApprovedDate = c.DateTimeOffset(precision: 7),
                        IsViewed = c.Boolean(nullable: false),
                        ViewedDate = c.DateTimeOffset(precision: 7),
                        IsRejected = c.Boolean(nullable: false),
                        RejectedDate = c.DateTimeOffset(precision: 7),
                        RejectReason = c.String(),
                        RejectedBy = c.String(),
                        UserAccepted = c.Boolean(nullable: false),
                        UserDeclined = c.Boolean(nullable: false),
                        UserAcceptedDate = c.DateTimeOffset(precision: 7),
                        UserDeclinedDate = c.DateTimeOffset(precision: 7),
                        DeclinedReason = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PROD_PropertyCustomerConsumingMapping",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Assignment = c.String(),
                        PropertyId = c.Long(nullable: false),
                        CustomerId = c.Long(nullable: false),
                        PropertyApplicationFormInstanceId = c.Long(nullable: false),
                        StartDate = c.DateTimeOffset(precision: 7),
                        ValidLeaseDurationInMonth = c.Int(),
                        IsCompleted = c.Boolean(nullable: false),
                        IsRenewable = c.Boolean(nullable: false),
                        NotifyCustomerBeforeExpire = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PROD_Property", t => t.PropertyId, cascadeDelete: true)
                .ForeignKey("dbo.PROD_PropertyApplicationFormInstance", t => t.PropertyApplicationFormInstanceId, cascadeDelete: true)
                .Index(t => t.PropertyId)
                .Index(t => t.PropertyApplicationFormInstanceId);
            
            CreateTable(
                "dbo.PROD_Property",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Status = c.String(),
                        PropertySellType = c.String(),
                        Name = c.String(),
                        CategoryId = c.Long(),
                        Description = c.String(),
                        ShortDescription = c.String(),
                        ExternalKey = c.String(),
                        Sku = c.String(),
                        Upc = c.String(),
                        UnitUpc = c.String(),
                        ParentPropertyId = c.Long(),
                        Price = c.Decimal(precision: 18, scale: 2),
                        UnitPrice = c.Decimal(precision: 18, scale: 2),
                        IsFeatured = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        OwnerId = c.Long(),
                        LocationId = c.Long(),
                        UnitNumber = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PROD_PropertyCategory", t => t.CategoryId)
                .ForeignKey("dbo.PROD_Property", t => t.ParentPropertyId)
                .Index(t => t.CategoryId)
                .Index(t => t.ParentPropertyId);
            
            CreateTable(
                "dbo.PROD_PropertyCategory",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        ImageUrl = c.String(),
                        DisplayOnMenu = c.Boolean(nullable: false),
                        DisplayOnHomePage = c.Boolean(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                        EnableSimilarProperty = c.Boolean(nullable: false),
                        ParentCategoryId = c.Long(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PROD_PropertyCategory", t => t.ParentCategoryId)
                .Index(t => t.ParentCategoryId);
            
            CreateTable(
                "dbo.PROD_PropertyDynamicPropertyCategory",
                c => new
                    {
                        CategoryId = c.Long(nullable: false),
                        DynamicPropertyId = c.Long(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CategoryId, t.DynamicPropertyId })
                .ForeignKey("dbo.PROD_PropertyCategory", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.PROD_PropertyImage",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Url = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                        IsMainImage = c.Boolean(nullable: false),
                        PropertyId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PROD_Property", t => t.PropertyId)
                .Index(t => t.PropertyId, name: "IX_Property_Id");
            
            CreateTable(
                "dbo.PROD_Tag",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        ParentTagId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PROD_Tag", t => t.ParentTagId)
                .Index(t => t.ParentTagId, name: "IX_ParentTag_Id");
            
            CreateTable(
                "dbo.PROD_PropertyCategoryFormConfiguration",
                c => new
                    {
                        PropertyCategoryId = c.Long(nullable: false),
                        FormConfigurationId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.PropertyCategoryId, t.FormConfigurationId })
                .ForeignKey("dbo.PROD_PropertyCategory", t => t.PropertyCategoryId, cascadeDelete: true)
                .Index(t => t.PropertyCategoryId);
            
            CreateTable(
                "dbo.PROD_TagProperty",
                c => new
                    {
                        TagId = c.Long(nullable: false),
                        PropertyId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.TagId, t.PropertyId })
                .ForeignKey("dbo.PROD_Tag", t => t.TagId, cascadeDelete: true)
                .ForeignKey("dbo.PROD_Property", t => t.PropertyId, cascadeDelete: true)
                .Index(t => t.TagId, name: "IX_Tag_Id")
                .Index(t => t.PropertyId, name: "IX_Property_Id");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PROD_PropertyCategoryFormConfiguration", "PropertyCategoryId", "dbo.PROD_PropertyCategory");
            DropForeignKey("dbo.PROD_PropertyCustomerConsumingMapping", "PropertyApplicationFormInstanceId", "dbo.PROD_PropertyApplicationFormInstance");
            DropForeignKey("dbo.PROD_PropertyCustomerConsumingMapping", "PropertyId", "dbo.PROD_Property");
            DropForeignKey("dbo.PROD_TagProperty", "PropertyId", "dbo.PROD_Property");
            DropForeignKey("dbo.PROD_TagProperty", "TagId", "dbo.PROD_Tag");
            DropForeignKey("dbo.PROD_Tag", "ParentTagId", "dbo.PROD_Tag");
            DropForeignKey("dbo.PROD_Property", "ParentPropertyId", "dbo.PROD_Property");
            DropForeignKey("dbo.PROD_PropertyImage", "PropertyId", "dbo.PROD_Property");
            DropForeignKey("dbo.PROD_Property", "CategoryId", "dbo.PROD_PropertyCategory");
            DropForeignKey("dbo.PROD_PropertyDynamicPropertyCategory", "CategoryId", "dbo.PROD_PropertyCategory");
            DropForeignKey("dbo.PROD_PropertyCategory", "ParentCategoryId", "dbo.PROD_PropertyCategory");
            DropIndex("dbo.PROD_TagProperty", "IX_Property_Id");
            DropIndex("dbo.PROD_TagProperty", "IX_Tag_Id");
            DropIndex("dbo.PROD_PropertyCategoryFormConfiguration", new[] { "PropertyCategoryId" });
            DropIndex("dbo.PROD_Tag", "IX_ParentTag_Id");
            DropIndex("dbo.PROD_PropertyImage", "IX_Property_Id");
            DropIndex("dbo.PROD_PropertyDynamicPropertyCategory", new[] { "CategoryId" });
            DropIndex("dbo.PROD_PropertyCategory", new[] { "ParentCategoryId" });
            DropIndex("dbo.PROD_Property", new[] { "ParentPropertyId" });
            DropIndex("dbo.PROD_Property", new[] { "CategoryId" });
            DropIndex("dbo.PROD_PropertyCustomerConsumingMapping", new[] { "PropertyApplicationFormInstanceId" });
            DropIndex("dbo.PROD_PropertyCustomerConsumingMapping", new[] { "PropertyId" });
            DropTable("dbo.PROD_TagProperty");
            DropTable("dbo.PROD_PropertyCategoryFormConfiguration");
            DropTable("dbo.PROD_Tag");
            DropTable("dbo.PROD_PropertyImage");
            DropTable("dbo.PROD_PropertyDynamicPropertyCategory");
            DropTable("dbo.PROD_PropertyCategory");
            DropTable("dbo.PROD_Property");
            DropTable("dbo.PROD_PropertyCustomerConsumingMapping");
            DropTable("dbo.PROD_PropertyApplicationFormInstance");
            DropTable("dbo.PROD_PropertyApplicationFormDocumentConfig");
        }
    }
}
