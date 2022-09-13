namespace LocationService.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_MigrationChanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Location", "CreatedBy", c => c.String());
            AlterColumn("dbo.Location", "ModifiedBy", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Location", "ModifiedBy", c => c.String(maxLength: 30));
            AlterColumn("dbo.Location", "CreatedBy", c => c.String(maxLength: 30));
        }
    }
}
