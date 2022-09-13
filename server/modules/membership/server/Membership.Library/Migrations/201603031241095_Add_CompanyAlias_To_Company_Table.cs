using System.Data.Entity.Migrations;

namespace Membership.Library.Migrations
{
    public partial class Add_CompanyAlias_To_Company_Table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Company", "Alias", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Company", "Alias");
        }
    }
}
