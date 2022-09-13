namespace MembershipLocation.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initialize_MembershipLocation_Db : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserLocation",
                c => new
                    {
                        UserId = c.Long(nullable: false),
                        LocationId = c.Long(nullable: false),
                        LocationTypeId = c.Long(),
                    })
                .PrimaryKey(t => new { t.UserId, t.LocationId })
                .ForeignKey("dbo.LocationType", t => t.LocationTypeId)
                .Index(t => t.LocationTypeId);
            
            CreateTable(
                "dbo.LocationType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Code = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserLocation", "LocationTypeId", "dbo.LocationType");
            DropIndex("dbo.UserLocation", new[] { "LocationTypeId" });
            DropTable("dbo.LocationType");
            DropTable("dbo.UserLocation");
        }
    }
}
