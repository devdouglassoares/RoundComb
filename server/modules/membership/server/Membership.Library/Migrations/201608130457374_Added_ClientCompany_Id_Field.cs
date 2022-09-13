namespace Membership.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_ClientCompany_Id_Field : DbMigration
    {
        public override void Up()
        {
            RenameIndex(table: "dbo.User", name: "IX_ClientCompany_Id", newName: "IX_ClientCompanyId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.User", name: "IX_ClientCompanyId", newName: "IX_ClientCompany_Id");
        }
    }
}
