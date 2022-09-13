namespace EntityReviews.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ConnectedEntityToReview : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EntityReview_EntityReview", "ConnectedEntityId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EntityReview_EntityReview", "ConnectedEntityId");
        }
    }
}
