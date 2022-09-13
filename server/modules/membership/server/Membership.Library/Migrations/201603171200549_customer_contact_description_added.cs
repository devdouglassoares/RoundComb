using System.Data.Entity.Migrations;

namespace Membership.Library.Migrations
{
    public partial class customer_contact_description_added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerContact", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerContact", "Description");
        }
    }
}
