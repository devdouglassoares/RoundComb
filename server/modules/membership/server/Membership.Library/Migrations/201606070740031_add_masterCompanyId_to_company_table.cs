namespace Membership.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_masterCompanyId_to_company_table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Company", "MasterCompanyId", c => c.Long());
            CreateIndex("dbo.Company", "MasterCompanyId", name: "IX_MasterCompany_Id");
            AddForeignKey("dbo.Company", "MasterCompanyId", "dbo.Company", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Company", "MasterCompanyId", "dbo.Company");
            DropIndex("dbo.Company", "IX_MasterCompany_Id");
            DropColumn("dbo.Company", "MasterCompanyId");
        }
    }
}
