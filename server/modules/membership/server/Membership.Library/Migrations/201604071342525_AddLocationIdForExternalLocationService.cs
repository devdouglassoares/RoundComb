namespace Membership.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLocationIdForExternalLocationService : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfile", "LocationId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfile", "LocationId");
        }
    }
}
