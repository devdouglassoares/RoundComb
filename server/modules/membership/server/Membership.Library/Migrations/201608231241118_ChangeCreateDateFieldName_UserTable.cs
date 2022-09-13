namespace Membership.Library.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ChangeCreateDateFieldName_UserTable : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.User", "CreateDate", "CreatedDate");
        }

        public override void Down()
        {
            RenameColumn("dbo.User", "CreatedDate", "CreateDate");
        }
    }
}
