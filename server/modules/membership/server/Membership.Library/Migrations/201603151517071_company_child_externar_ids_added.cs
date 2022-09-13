using System.Data.Entity.Migrations;

namespace Membership.Library.Migrations
{
    public partial class company_child_externar_ids_added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerContact", "ExternalId", c => c.Int(nullable: false));
            AddColumn("dbo.CustomerPartner", "ExternalId", c => c.Int(nullable: false));
            AddColumn("dbo.CustomerVendor", "ExternalId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerVendor", "ExternalId");
            DropColumn("dbo.CustomerPartner", "ExternalId");
            DropColumn("dbo.CustomerContact", "ExternalId");
        }
    }
}
