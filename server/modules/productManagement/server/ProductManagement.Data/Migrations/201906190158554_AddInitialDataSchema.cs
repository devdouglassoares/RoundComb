namespace ProductManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInitialDataSchema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PROD_FeaturedCategory",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CategoryId = c.Long(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PROD_PropertyCategory", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
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
                "dbo.PROD_PropertyCart",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        DelegateToUserId = c.Long(),
                        CheckedOut = c.Boolean(nullable: false),
                        CheckedOutDate = c.DateTimeOffset(precision: 7),
                        Closed = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PROD_PropertyCartItem",
                c => new
                    {
                        PropertyId = c.Long(nullable: false),
                        PropertyCartId = c.Long(nullable: false),
                        Quantity = c.Int(nullable: false),
                        AppliedPrice = c.Decimal(precision: 18, scale: 2),
                        AppliedUnitPrice = c.Decimal(precision: 18, scale: 2),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => new { t.PropertyId, t.PropertyCartId })
                .ForeignKey("dbo.PROD_Property", t => t.PropertyId, cascadeDelete: true)
                .ForeignKey("dbo.PROD_PropertyCart", t => t.PropertyCartId, cascadeDelete: true)
                .Index(t => t.PropertyId)
                .Index(t => t.PropertyCartId);
            
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
                "dbo.PROD_PropertyImage",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Url = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                        IsMainImage = c.Boolean(nullable: false),
                        PropertyId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PROD_Property", t => t.PropertyId, cascadeDelete: true)
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
                "dbo.PROD_PropertyContactMessage",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        OwnerId = c.Long(nullable: false),
                        DestinationUserId = c.Long(),
                        ThreadGuid = c.Guid(nullable: false),
                        Title = c.String(maxLength: 200),
                        Message = c.String(),
                        PropertyId = c.Long(),
                        ReplyToMessageId = c.Long(),
                        SentDate = c.DateTimeOffset(precision: 7),
                        IsViewed = c.Boolean(nullable: false),
                        ViewedDate = c.DateTimeOffset(precision: 7),
                        RepliedDate = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PROD_Property", t => t.PropertyId)
                .ForeignKey("dbo.PROD_PropertyContactMessage", t => t.ReplyToMessageId)
                .Index(t => t.PropertyId)
                .Index(t => t.ReplyToMessageId);
            
            CreateTable(
                "dbo.PROD_PropertyHistory",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PropertyId = c.Long(nullable: false),
                        UserId = c.Long(),
                        PropertyHistoryType = c.String(),
                        Comment = c.String(),
                        RecordDate = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PROD_Property", t => t.PropertyId, cascadeDelete: true)
                .Index(t => t.PropertyId);
            
            CreateTable(
                "dbo.PROD_PropertyImportMappingSet",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        Configuration = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PROD_PropertySearchRecord",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        SearchQuery = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PROD_ServiceProviding",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PROD_Vendor",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        ContactPhoneNumber = c.String(),
                        Fax = c.String(),
                        EmailAddress = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Country = c.String(),
                        ZipCode = c.String(),
                        FormattedAddress = c.String(),
                        ProfilePhotoUrl = c.String(),
                        Description = c.String(),
                        UserId = c.Long(nullable: false),
                        LocationId = c.Long(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PROD_VendorRequest",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FromUserId = c.Long(nullable: false),
                        ToUserId = c.Long(nullable: false),
                        PropertyId = c.Long(nullable: false),
                        RequestSentDate = c.DateTimeOffset(nullable: false, precision: 7),
                        IsAccepted = c.Boolean(nullable: false),
                        RequestAcceptedDate = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PROD_Property", t => t.PropertyId, cascadeDelete: true)
                .Index(t => t.PropertyId);
            
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
            
            CreateTable(
                "dbo.PROD_VendorServiceProviding",
                c => new
                    {
                        VendorId = c.Long(nullable: false),
                        ServiceProvidingId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.VendorId, t.ServiceProvidingId })
                .ForeignKey("dbo.PROD_Vendor", t => t.VendorId, cascadeDelete: true)
                .ForeignKey("dbo.PROD_ServiceProviding", t => t.ServiceProvidingId, cascadeDelete: true)
                .Index(t => t.VendorId, name: "IX_Vendor_Id")
                .Index(t => t.ServiceProvidingId, name: "IX_ServiceProviding_Id");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PROD_VendorRequest", "PropertyId", "dbo.PROD_Property");
            DropForeignKey("dbo.PROD_VendorServiceProviding", "ServiceProvidingId", "dbo.PROD_ServiceProviding");
            DropForeignKey("dbo.PROD_VendorServiceProviding", "VendorId", "dbo.PROD_Vendor");
            DropForeignKey("dbo.PROD_PropertyHistory", "PropertyId", "dbo.PROD_Property");
            DropForeignKey("dbo.PROD_PropertyContactMessage", "ReplyToMessageId", "dbo.PROD_PropertyContactMessage");
            DropForeignKey("dbo.PROD_PropertyContactMessage", "PropertyId", "dbo.PROD_Property");
            DropForeignKey("dbo.PROD_PropertyCartItem", "PropertyCartId", "dbo.PROD_PropertyCart");
            DropForeignKey("dbo.PROD_PropertyCartItem", "PropertyId", "dbo.PROD_Property");
            DropForeignKey("dbo.PROD_TagProperty", "PropertyId", "dbo.PROD_Property");
            DropForeignKey("dbo.PROD_TagProperty", "TagId", "dbo.PROD_Tag");
            DropForeignKey("dbo.PROD_Tag", "ParentTagId", "dbo.PROD_Tag");
            DropForeignKey("dbo.PROD_Property", "ParentPropertyId", "dbo.PROD_Property");
            DropForeignKey("dbo.PROD_PropertyImage", "PropertyId", "dbo.PROD_Property");
            DropForeignKey("dbo.PROD_Property", "CategoryId", "dbo.PROD_PropertyCategory");
            DropForeignKey("dbo.PROD_FeaturedCategory", "CategoryId", "dbo.PROD_PropertyCategory");
            DropForeignKey("dbo.PROD_PropertyCategory", "ParentCategoryId", "dbo.PROD_PropertyCategory");
            DropForeignKey("dbo.PROD_PropertyDynamicPropertyCategory", "CategoryId", "dbo.PROD_PropertyCategory");
            DropIndex("dbo.PROD_VendorServiceProviding", "IX_ServiceProviding_Id");
            DropIndex("dbo.PROD_VendorServiceProviding", "IX_Vendor_Id");
            DropIndex("dbo.PROD_TagProperty", "IX_Property_Id");
            DropIndex("dbo.PROD_TagProperty", "IX_Tag_Id");
            DropIndex("dbo.PROD_VendorRequest", new[] { "PropertyId" });
            DropIndex("dbo.PROD_PropertyHistory", new[] { "PropertyId" });
            DropIndex("dbo.PROD_PropertyContactMessage", new[] { "ReplyToMessageId" });
            DropIndex("dbo.PROD_PropertyContactMessage", new[] { "PropertyId" });
            DropIndex("dbo.PROD_Tag", "IX_ParentTag_Id");
            DropIndex("dbo.PROD_PropertyImage", "IX_Property_Id");
            DropIndex("dbo.PROD_Property", new[] { "ParentPropertyId" });
            DropIndex("dbo.PROD_Property", new[] { "CategoryId" });
            DropIndex("dbo.PROD_PropertyCartItem", new[] { "PropertyCartId" });
            DropIndex("dbo.PROD_PropertyCartItem", new[] { "PropertyId" });
            DropIndex("dbo.PROD_PropertyDynamicPropertyCategory", new[] { "CategoryId" });
            DropIndex("dbo.PROD_PropertyCategory", new[] { "ParentCategoryId" });
            DropIndex("dbo.PROD_FeaturedCategory", new[] { "CategoryId" });
            DropTable("dbo.PROD_VendorServiceProviding");
            DropTable("dbo.PROD_TagProperty");
            DropTable("dbo.PROD_VendorRequest");
            DropTable("dbo.PROD_Vendor");
            DropTable("dbo.PROD_ServiceProviding");
            DropTable("dbo.PROD_PropertySearchRecord");
            DropTable("dbo.PROD_PropertyImportMappingSet");
            DropTable("dbo.PROD_PropertyHistory");
            DropTable("dbo.PROD_PropertyContactMessage");
            DropTable("dbo.PROD_Tag");
            DropTable("dbo.PROD_PropertyImage");
            DropTable("dbo.PROD_Property");
            DropTable("dbo.PROD_PropertyCartItem");
            DropTable("dbo.PROD_PropertyCart");
            DropTable("dbo.PROD_PropertyDynamicPropertyCategory");
            DropTable("dbo.PROD_PropertyCategory");
            DropTable("dbo.PROD_FeaturedCategory");
        }
    }
}
