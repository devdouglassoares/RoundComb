namespace Common.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class announcement_audit_added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AnnouncementViewAudit",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AnnouncementKey = c.String(),
                        UserId = c.Long(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AnnouncementViewAudit");
        }
    }
}
