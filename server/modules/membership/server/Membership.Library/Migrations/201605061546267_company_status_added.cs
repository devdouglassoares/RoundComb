namespace Membership.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class company_status_added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Company", "StatusLastUpdated", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AddColumn("dbo.Company", "CurrentStatus", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Company", "CurrentStatus");
            DropColumn("dbo.Company", "StatusLastUpdated");
        }
    }
}
