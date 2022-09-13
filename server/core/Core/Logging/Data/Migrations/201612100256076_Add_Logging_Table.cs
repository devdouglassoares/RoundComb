namespace Core.Logging.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Logging_Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogEntry",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Date = c.DateTimeOffset(precision: 7),
                        Thread = c.String(),
                        Level = c.String(),
                        Logger = c.String(),
                        Message = c.String(),
                        Exception = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LogEntry");
        }
    }
}
