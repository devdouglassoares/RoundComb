namespace EntityReviews.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ModifiedDate_To_EntityReview : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EntityReview_EntityReview", "ModifiedDate", c => c.DateTimeOffset(precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EntityReview_EntityReview", "ModifiedDate");
        }
    }
}
