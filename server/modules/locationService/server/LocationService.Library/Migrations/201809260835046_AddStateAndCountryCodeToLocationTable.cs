namespace LocationService.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStateAndCountryCodeToLocationTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Location", "StateCode", c => c.String());
            AddColumn("dbo.Location", "CountryCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Location", "CountryCode");
            DropColumn("dbo.Location", "StateCode");
        }
    }
}
