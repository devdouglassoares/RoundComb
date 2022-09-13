namespace Membership.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class customer_child_entities_updated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerContact", "ExternalId", c => c.Int(nullable: false));
            AddColumn("dbo.CustomerContact", "Description", c => c.String());
            AddColumn("dbo.CustomerPartner", "ExternalId", c => c.Int(nullable: false));
            AddColumn("dbo.CustomerVendor", "ExternalId", c => c.Int(nullable: false));
            AddColumn("dbo.CustomerVendor", "Notes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerVendor", "Notes");
            DropColumn("dbo.CustomerVendor", "ExternalId");
            DropColumn("dbo.CustomerPartner", "ExternalId");
            DropColumn("dbo.CustomerContact", "Description");
            DropColumn("dbo.CustomerContact", "ExternalId");
        }
    }
}
