using System.Data.Entity.Migrations;

namespace Membership.Library.Migrations
{
    public partial class membership_init_migration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccessEntity",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Type = c.Int(nullable: false),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AccessModule",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RoleAccessRight",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AccessModuleId = c.Long(),
                        AccessRightId = c.Long(),
                        RoleId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AccessModule", t => t.AccessModuleId)
                .ForeignKey("dbo.AccessRight", t => t.AccessRightId)
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.AccessModuleId, name: "IX_AccessModule_Id")
                .Index(t => t.AccessRightId, name: "IX_AccessRight_Id")
                .Index(t => t.RoleId, name: "IX_Role_Id");
            
            CreateTable(
                "dbo.AccessRight",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AccessRightName = c.String(),
                        Priority = c.Int(nullable: false),
                        AccessKind = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Code = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Email = c.String(),
                        PasswordHash = c.String(),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        LastName = c.String(),
                        Comment = c.String(),
                        IsApproved = c.Boolean(nullable: false),
                        PasswordFailuresSinceLastSuccess = c.Int(nullable: false),
                        LastPasswordFailureDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        LastActivityDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        LastLockoutDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        LastLoginDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ConfirmationToken = c.String(),
                        CreateDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        IsLockedOut = c.Boolean(nullable: false),
                        LastPasswordChangeDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        PasswordVerificationToken = c.String(),
                        PasswordVerificationTokenExpirationDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                        HomePhoneNumber = c.String(),
                        CellPhoneNumber = c.String(),
                        ExternalKey = c.String(),
                        Address = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        Devices = c.String(),
                        CompanyId = c.Long(),
                        ClientCompanyId = c.Long(),
                        ImpersonatedAsId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.Company", t => t.ClientCompanyId)
                .ForeignKey("dbo.User", t => t.ImpersonatedAsId)
                .Index(t => t.CompanyId, name: "IX_Company_Id")
                .Index(t => t.ClientCompanyId, name: "IX_ClientCompany_Id")
                .Index(t => t.ImpersonatedAsId, name: "IX_ImpersonatedAs_Id");
            
            CreateTable(
                "dbo.Company",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Domain = c.String(),
                        Code = c.String(),
                        Name = c.String(),
                        LogoUrl = c.String(),
                        Address = c.String(),
                        PhoneNumber = c.String(),
                        CurrentTheme = c.String(),
                        ClientsLimit = c.Int(),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        LastModifiedBy = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        MainContactUserId = c.Long(),
                        OwnerId = c.Long(),
                        TmsAccountId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.MainContactUserId)
                .ForeignKey("dbo.User", t => t.OwnerId)
                .ForeignKey("dbo.Tms_Account", t => t.TmsAccountId)
                .Index(t => t.MainContactUserId, name: "IX_MainContactUser_Id")
                .Index(t => t.OwnerId, name: "IX_Owner_Id")
                .Index(t => t.TmsAccountId, name: "IX_TmsAccount_Id");
            
            CreateTable(
                "dbo.CompanyDocument",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FileRecordId = c.Long(nullable: false),
                        MasterId = c.Long(nullable: false),
                        Comment = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        CreatedBy = c.String(maxLength: 30),
                        ModifiedBy = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CompanyDocument_FileRecord", t => t.FileRecordId, cascadeDelete: true)
                .ForeignKey("dbo.Company", t => t.MasterId, cascadeDelete: true)
                .Index(t => t.FileRecordId)
                .Index(t => t.MasterId);
            
            CreateTable(
                "dbo.CompanyDocument_FileRecord",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FileName = c.String(),
                        FileUrl = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        CreatedBy = c.String(maxLength: 30),
                        ModifiedBy = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CompanySetting",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Value = c.String(),
                        CompanyId = c.Long(),
                        ParentId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .ForeignKey("dbo.CompanySetting", t => t.ParentId)
                .Index(t => t.CompanyId, name: "IX_Company_Id")
                .Index(t => t.ParentId, name: "IX_Parent_Id");
            
            CreateTable(
                "dbo.Tms_Account",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        JiraUrl = c.String(),
                        ProjectKey = c.String(),
                        Login = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Contact",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Type = c.String(),
                        Value = c.String(),
                        UserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId, name: "IX_User_Id");
            
            CreateTable(
                "dbo.UserExternalLogin",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ExternalProviderName = c.String(),
                        AccessKey = c.String(),
                        SecretKey = c.String(),
                        UserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId, name: "IX_User_Id");
            
            CreateTable(
                "dbo.Group",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        CompanyId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .Index(t => t.CompanyId, name: "IX_Company_Id");
            
            CreateTable(
                "dbo.UserAccessRight",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AccessModuleId = c.Long(),
                        AccessRightId = c.Long(),
                        UserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AccessModule", t => t.AccessModuleId)
                .ForeignKey("dbo.AccessRight", t => t.AccessRightId)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.AccessModuleId, name: "IX_AccessModule_Id")
                .Index(t => t.AccessRightId, name: "IX_AccessRight_Id")
                .Index(t => t.UserId, name: "IX_User_Id");
            
            CreateTable(
                "dbo.AccessLog",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AccessLogId = c.Int(nullable: false),
                        UserId = c.Int(),
                        PersonId = c.Int(),
                        UserName = c.String(),
                        FullName = c.String(),
                        FeatureID = c.Int(),
                        FeatureName = c.String(),
                        UrlAccessed = c.String(),
                        LOGON_USER = c.String(),
                        AUTH_USER = c.String(),
                        LOCAL_ADDR = c.String(),
                        REMOTE_ADDR = c.String(),
                        REMOTE_HOST = c.String(),
                        WebType = c.String(),
                        AccessedDateTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApplicationLogging",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TimeStamp = c.DateTime(precision: 7, storeType: "datetime2"),
                        Message = c.String(),
                        Level = c.String(),
                        Logger = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApplicationPermission",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Key = c.String(),
                        Value = c.String(),
                        IsEnabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConnectionInfo",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Content = c.String(),
                        CompanyId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .Index(t => t.CompanyId, name: "IX_Company_Id");
            
            CreateTable(
                "dbo.ConnectionInfoPinNumber",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Pin = c.Int(nullable: false),
                        UserId = c.Long(nullable: false),
                        ConnectionInfoId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ConnectionInfo", t => t.ConnectionInfoId)
                .Index(t => t.ConnectionInfoId, name: "IX_ConnectionInfo_Id");
            
            CreateTable(
                "dbo.CustomerContact",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        CompanyId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .Index(t => t.CompanyId, name: "IX_Company_Id");
            
            CreateTable(
                "dbo.CustomerPartner",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CompanyId = c.Long(),
                        PartnerId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .ForeignKey("dbo.Partner", t => t.PartnerId)
                .Index(t => t.CompanyId, name: "IX_Company_Id")
                .Index(t => t.PartnerId, name: "IX_Partner_Id");
            
            CreateTable(
                "dbo.Partner",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Type = c.String(),
                        ExternalId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomerVendor",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CompanyId = c.Long(),
                        ProjectDescriptionId = c.Long(),
                        SiteId = c.Long(),
                        VendorId = c.Long(),
                        VendorTypeId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .ForeignKey("dbo.ProjectDescription", t => t.ProjectDescriptionId)
                .ForeignKey("dbo.CustomerSite", t => t.SiteId)
                .ForeignKey("dbo.Vendor", t => t.VendorId)
                .ForeignKey("dbo.VendorType", t => t.VendorTypeId)
                .Index(t => t.CompanyId, name: "IX_Company_Id")
                .Index(t => t.ProjectDescriptionId, name: "IX_ProjectDescription_Id")
                .Index(t => t.SiteId, name: "IX_Site_Id")
                .Index(t => t.VendorId, name: "IX_Vendor_Id")
                .Index(t => t.VendorTypeId, name: "IX_VendorType_Id");
            
            CreateTable(
                "dbo.ProjectDescription",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(),
                        ExternalId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomerSite",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Street = c.String(),
                        Street2 = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Zipcode = c.Int(),
                        IsPrimary = c.Boolean(),
                        CompanyId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .Index(t => t.CompanyId, name: "IX_Company_Id");
            
            CreateTable(
                "dbo.Vendor",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        ExternalId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VendorType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TypeName = c.String(),
                        ExternalId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomerViewAudit",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CompanyId = c.Long(nullable: false),
                        UserId = c.Long(nullable: false),
                        Date = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SiteSetting",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SettingKey = c.String(),
                        Value = c.String(),
                        LastUpdated = c.DateTimeOffset(precision: 7),
                        LastUpdatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ContactName = c.String(),
                        ProfilePhotoUrl = c.String(),
                        MarriageStatus = c.String(),
                        Gender = c.String(),
                        Birthday = c.DateTimeOffset(precision: 7),
                        ProfileSummary = c.String(),
                        PhoneNumber = c.String(),
                        ContactPhoneNumber = c.String(),
                        Address = c.String(),
                        Address2 = c.String(),
                        State = c.String(),
                        City = c.String(),
                        Country = c.String(),
                        ZipCode = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTimeOffset(precision: 7),
                        UserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserRoleProfileProperties",
                c => new
                    {
                        UserRoleId = c.Long(nullable: false),
                        PropertyId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserRoleId, t.PropertyId })
                .ForeignKey("dbo.Role", t => t.UserRoleId, cascadeDelete: true)
                .Index(t => t.UserRoleId);
            
            CreateTable(
                "dbo.AccessModuleAccessEntity",
                c => new
                    {
                        AccessModuleId = c.Long(nullable: false),
                        AccessEntityId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.AccessModuleId, t.AccessEntityId })
                .ForeignKey("dbo.AccessModule", t => t.AccessModuleId, cascadeDelete: true)
                .ForeignKey("dbo.AccessEntity", t => t.AccessEntityId, cascadeDelete: true)
                .Index(t => t.AccessModuleId, name: "IX_AccessModule_Id")
                .Index(t => t.AccessEntityId, name: "IX_AccessEntity_Id");
            
            CreateTable(
                "dbo.FollowedUsers",
                c => new
                    {
                        UserId = c.Long(nullable: false),
                        FollowedUserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.FollowedUserId })
                .ForeignKey("dbo.User", t => t.UserId)
                .ForeignKey("dbo.User", t => t.FollowedUserId)
                .Index(t => t.UserId)
                .Index(t => t.FollowedUserId);
            
            CreateTable(
                "dbo.GroupUser",
                c => new
                    {
                        UserId = c.Long(nullable: false),
                        GroupId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.GroupId })
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Group", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.UserId, name: "IX_User_Id")
                .Index(t => t.GroupId, name: "IX_Group_Id");
            
            CreateTable(
                "dbo.RoleUser",
                c => new
                    {
                        RoleId = c.Long(nullable: false),
                        UserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId, name: "IX_Role_Id")
                .Index(t => t.UserId, name: "IX_User_Id");
            
            CreateTable(
                "dbo.Feature",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        Code = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AccessEntity", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.SiteUrl",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        Url = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AccessEntity", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SiteUrl", "Id", "dbo.AccessEntity");
            DropForeignKey("dbo.Feature", "Id", "dbo.AccessEntity");
            DropForeignKey("dbo.UserRoleProfileProperties", "UserRoleId", "dbo.Role");
            DropForeignKey("dbo.UserProfile", "UserId", "dbo.User");
            DropForeignKey("dbo.CustomerVendor", "VendorTypeId", "dbo.VendorType");
            DropForeignKey("dbo.CustomerVendor", "VendorId", "dbo.Vendor");
            DropForeignKey("dbo.CustomerVendor", "SiteId", "dbo.CustomerSite");
            DropForeignKey("dbo.CustomerSite", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.CustomerVendor", "ProjectDescriptionId", "dbo.ProjectDescription");
            DropForeignKey("dbo.CustomerVendor", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.CustomerPartner", "PartnerId", "dbo.Partner");
            DropForeignKey("dbo.CustomerPartner", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.CustomerContact", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.ConnectionInfoPinNumber", "ConnectionInfoId", "dbo.ConnectionInfo");
            DropForeignKey("dbo.ConnectionInfo", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.RoleUser", "UserId", "dbo.User");
            DropForeignKey("dbo.RoleUser", "RoleId", "dbo.Role");
            DropForeignKey("dbo.UserAccessRight", "UserId", "dbo.User");
            DropForeignKey("dbo.UserAccessRight", "AccessRightId", "dbo.AccessRight");
            DropForeignKey("dbo.UserAccessRight", "AccessModuleId", "dbo.AccessModule");
            DropForeignKey("dbo.User", "ImpersonatedAsId", "dbo.User");
            DropForeignKey("dbo.GroupUser", "GroupId", "dbo.Group");
            DropForeignKey("dbo.GroupUser", "UserId", "dbo.User");
            DropForeignKey("dbo.Group", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.FollowedUsers", "FollowedUserId", "dbo.User");
            DropForeignKey("dbo.FollowedUsers", "UserId", "dbo.User");
            DropForeignKey("dbo.UserExternalLogin", "UserId", "dbo.User");
            DropForeignKey("dbo.Contact", "UserId", "dbo.User");
            DropForeignKey("dbo.User", "ClientCompanyId", "dbo.Company");
            DropForeignKey("dbo.User", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.Company", "TmsAccountId", "dbo.Tms_Account");
            DropForeignKey("dbo.CompanySetting", "ParentId", "dbo.CompanySetting");
            DropForeignKey("dbo.CompanySetting", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.Company", "OwnerId", "dbo.User");
            DropForeignKey("dbo.Company", "MainContactUserId", "dbo.User");
            DropForeignKey("dbo.CompanyDocument", "MasterId", "dbo.Company");
            DropForeignKey("dbo.CompanyDocument", "FileRecordId", "dbo.CompanyDocument_FileRecord");
            DropForeignKey("dbo.RoleAccessRight", "RoleId", "dbo.Role");
            DropForeignKey("dbo.RoleAccessRight", "AccessRightId", "dbo.AccessRight");
            DropForeignKey("dbo.RoleAccessRight", "AccessModuleId", "dbo.AccessModule");
            DropForeignKey("dbo.AccessModuleAccessEntity", "AccessEntityId", "dbo.AccessEntity");
            DropForeignKey("dbo.AccessModuleAccessEntity", "AccessModuleId", "dbo.AccessModule");
            DropIndex("dbo.SiteUrl", new[] { "Id" });
            DropIndex("dbo.Feature", new[] { "Id" });
            DropIndex("dbo.RoleUser", "IX_User_Id");
            DropIndex("dbo.RoleUser", "IX_Role_Id");
            DropIndex("dbo.GroupUser", "IX_Group_Id");
            DropIndex("dbo.GroupUser", "IX_User_Id");
            DropIndex("dbo.FollowedUsers", new[] { "FollowedUserId" });
            DropIndex("dbo.FollowedUsers", new[] { "UserId" });
            DropIndex("dbo.AccessModuleAccessEntity", "IX_AccessEntity_Id");
            DropIndex("dbo.AccessModuleAccessEntity", "IX_AccessModule_Id");
            DropIndex("dbo.UserRoleProfileProperties", new[] { "UserRoleId" });
            DropIndex("dbo.UserProfile", new[] { "UserId" });
            DropIndex("dbo.CustomerSite", "IX_Company_Id");
            DropIndex("dbo.CustomerVendor", "IX_VendorType_Id");
            DropIndex("dbo.CustomerVendor", "IX_Vendor_Id");
            DropIndex("dbo.CustomerVendor", "IX_Site_Id");
            DropIndex("dbo.CustomerVendor", "IX_ProjectDescription_Id");
            DropIndex("dbo.CustomerVendor", "IX_Company_Id");
            DropIndex("dbo.CustomerPartner", "IX_Partner_Id");
            DropIndex("dbo.CustomerPartner", "IX_Company_Id");
            DropIndex("dbo.CustomerContact", "IX_Company_Id");
            DropIndex("dbo.ConnectionInfoPinNumber", "IX_ConnectionInfo_Id");
            DropIndex("dbo.ConnectionInfo", "IX_Company_Id");
            DropIndex("dbo.UserAccessRight", "IX_User_Id");
            DropIndex("dbo.UserAccessRight", "IX_AccessRight_Id");
            DropIndex("dbo.UserAccessRight", "IX_AccessModule_Id");
            DropIndex("dbo.Group", "IX_Company_Id");
            DropIndex("dbo.UserExternalLogin", "IX_User_Id");
            DropIndex("dbo.Contact", "IX_User_Id");
            DropIndex("dbo.CompanySetting", "IX_Parent_Id");
            DropIndex("dbo.CompanySetting", "IX_Company_Id");
            DropIndex("dbo.CompanyDocument", new[] { "MasterId" });
            DropIndex("dbo.CompanyDocument", new[] { "FileRecordId" });
            DropIndex("dbo.Company", "IX_TmsAccount_Id");
            DropIndex("dbo.Company", "IX_Owner_Id");
            DropIndex("dbo.Company", "IX_MainContactUser_Id");
            DropIndex("dbo.User", "IX_ImpersonatedAs_Id");
            DropIndex("dbo.User", "IX_ClientCompany_Id");
            DropIndex("dbo.User", "IX_Company_Id");
            DropIndex("dbo.RoleAccessRight", "IX_Role_Id");
            DropIndex("dbo.RoleAccessRight", "IX_AccessRight_Id");
            DropIndex("dbo.RoleAccessRight", "IX_AccessModule_Id");
            DropTable("dbo.SiteUrl");
            DropTable("dbo.Feature");
            DropTable("dbo.RoleUser");
            DropTable("dbo.GroupUser");
            DropTable("dbo.FollowedUsers");
            DropTable("dbo.AccessModuleAccessEntity");
            DropTable("dbo.UserRoleProfileProperties");
            DropTable("dbo.UserProfile");
            DropTable("dbo.SiteSetting");
            DropTable("dbo.CustomerViewAudit");
            DropTable("dbo.VendorType");
            DropTable("dbo.Vendor");
            DropTable("dbo.CustomerSite");
            DropTable("dbo.ProjectDescription");
            DropTable("dbo.CustomerVendor");
            DropTable("dbo.Partner");
            DropTable("dbo.CustomerPartner");
            DropTable("dbo.CustomerContact");
            DropTable("dbo.ConnectionInfoPinNumber");
            DropTable("dbo.ConnectionInfo");
            DropTable("dbo.ApplicationPermission");
            DropTable("dbo.ApplicationLogging");
            DropTable("dbo.AccessLog");
            DropTable("dbo.UserAccessRight");
            DropTable("dbo.Group");
            DropTable("dbo.UserExternalLogin");
            DropTable("dbo.Contact");
            DropTable("dbo.Tms_Account");
            DropTable("dbo.CompanySetting");
            DropTable("dbo.CompanyDocument_FileRecord");
            DropTable("dbo.CompanyDocument");
            DropTable("dbo.Company");
            DropTable("dbo.User");
            DropTable("dbo.Role");
            DropTable("dbo.AccessRight");
            DropTable("dbo.RoleAccessRight");
            DropTable("dbo.AccessModule");
            DropTable("dbo.AccessEntity");
        }
    }
}
