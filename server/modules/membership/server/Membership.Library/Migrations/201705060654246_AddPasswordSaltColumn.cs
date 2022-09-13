namespace Membership.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPasswordSaltColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "PasswordSalt", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "PasswordSalt");
        }
    }
}
