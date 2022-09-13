namespace Membership.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Audit_Fields_To_User_Table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "ModifiedDate", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.User", "CreatedBy", c => c.String());
            AddColumn("dbo.User", "ModifiedBy", c => c.String());
            AlterColumn("dbo.User", "CreatedDate", c => c.DateTimeOffset(precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.User", "CreatedDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            DropColumn("dbo.User", "ModifiedBy");
            DropColumn("dbo.User", "CreatedBy");
            DropColumn("dbo.User", "ModifiedDate");
        }
    }
}
