namespace Membership.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_DatetimeNullable_For_User : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.User", "LastPasswordChangeDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.User", "LastPasswordChangeDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
    }
}
