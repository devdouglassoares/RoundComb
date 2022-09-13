namespace Membership.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class status_valid_date_added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Company", "Status", c => c.String());
            AddColumn("dbo.Company", "StatusValidDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            DropColumn("dbo.Company", "CurrentStatus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Company", "CurrentStatus", c => c.String());
            DropColumn("dbo.Company", "StatusValidDate");
            DropColumn("dbo.Company", "Status");
        }
    }
}
