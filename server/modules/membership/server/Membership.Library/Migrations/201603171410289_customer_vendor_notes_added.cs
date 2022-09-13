using System.Data.Entity.Migrations;

namespace Membership.Library.Migrations
{
    public partial class customer_vendor_notes_added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerVendor", "Notes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerVendor", "Notes");
        }
    }
}
