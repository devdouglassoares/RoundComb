namespace Membership.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConfigureMaster_Children_Companies : DbMigration
    {
        public override void Up()
        {
            RenameIndex(table: "dbo.User", name: "IX_Company_Id", newName: "IX_CompanyId");
            RenameIndex(table: "dbo.Company", name: "IX_MasterCompany_Id", newName: "IX_MasterCompanyId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Company", name: "IX_MasterCompanyId", newName: "IX_MasterCompany_Id");
            RenameIndex(table: "dbo.User", name: "IX_CompanyId", newName: "IX_Company_Id");
        }
    }
}
