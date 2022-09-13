namespace Common.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ExceptionLogger : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExceptionLogger",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Data = c.String(),
                        Message = c.String(),
                        ExceptionType = c.String(),
                        DateTime = c.DateTimeOffset(precision: 7),
                        Notified = c.DateTimeOffset(precision: 7),
                        NotifiedTo = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AnnouncementViewAudit", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.AnnouncementViewAudit", "CreatedDate", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.AnnouncementViewAudit", "ModifiedDate", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.AnnouncementViewAudit", "CreatedBy", c => c.String());
            AddColumn("dbo.AnnouncementViewAudit", "ModifiedBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AnnouncementViewAudit", "ModifiedBy");
            DropColumn("dbo.AnnouncementViewAudit", "CreatedBy");
            DropColumn("dbo.AnnouncementViewAudit", "ModifiedDate");
            DropColumn("dbo.AnnouncementViewAudit", "CreatedDate");
            DropColumn("dbo.AnnouncementViewAudit", "IsDeleted");
            DropTable("dbo.ExceptionLogger");
        }
    }
}
