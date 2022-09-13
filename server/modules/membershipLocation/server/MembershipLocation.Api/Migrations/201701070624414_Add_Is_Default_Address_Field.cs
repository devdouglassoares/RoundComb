namespace MembershipLocation.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Is_Default_Address_Field : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserLocation", "IsDefaultAddress", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserLocation", "IsDefaultAddress");
        }
    }
}
