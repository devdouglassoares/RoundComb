namespace Membership.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_RegistrationDate_Field_To_UserTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "RegistrationDate", c => c.DateTimeOffset(precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "RegistrationDate");
        }
    }
}
