namespace Membership.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Correct_AuditFields_Max_Length : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CompanyDocument", "CreatedBy", c => c.String());
            AlterColumn("dbo.CompanyDocument", "ModifiedBy", c => c.String());
            AlterColumn("dbo.CompanyDocument_FileRecord", "CreatedBy", c => c.String());
            AlterColumn("dbo.CompanyDocument_FileRecord", "ModifiedBy", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CompanyDocument_FileRecord", "ModifiedBy", c => c.String(maxLength: 30));
            AlterColumn("dbo.CompanyDocument_FileRecord", "CreatedBy", c => c.String(maxLength: 30));
            AlterColumn("dbo.CompanyDocument", "ModifiedBy", c => c.String(maxLength: 30));
            AlterColumn("dbo.CompanyDocument", "CreatedBy", c => c.String(maxLength: 30));
        }
    }
}
