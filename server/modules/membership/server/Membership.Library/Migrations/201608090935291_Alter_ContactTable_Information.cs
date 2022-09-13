namespace Membership.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Alter_ContactTable_Information : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Contact", "UserId", "dbo.User");
            DropIndex("dbo.Contact", "IX_User_Id");
            AlterColumn("dbo.Contact", "UserId", c => c.Long(nullable: false));
            CreateIndex("dbo.Contact", "UserId");
            AddForeignKey("dbo.Contact", "UserId", "dbo.User", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contact", "UserId", "dbo.User");
            DropIndex("dbo.Contact", new[] { "UserId" });
            AlterColumn("dbo.Contact", "UserId", c => c.Long());
            CreateIndex("dbo.Contact", "UserId", name: "IX_User_Id");
            AddForeignKey("dbo.Contact", "UserId", "dbo.User", "Id");
        }
    }
}
