namespace EntityReviews.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Ability_To_Reply_To_Review : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EntityReview_EntityReview", "RepliedToReviewId", c => c.Long());
            AlterColumn("dbo.EntityReview_EntityReview", "ReviewerUserId", c => c.Long());
            CreateIndex("dbo.EntityReview_EntityReview", "RepliedToReviewId");
            AddForeignKey("dbo.EntityReview_EntityReview", "RepliedToReviewId", "dbo.EntityReview_EntityReview", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EntityReview_EntityReview", "RepliedToReviewId", "dbo.EntityReview_EntityReview");
            DropIndex("dbo.EntityReview_EntityReview", new[] { "RepliedToReviewId" });
            AlterColumn("dbo.EntityReview_EntityReview", "ReviewerUserId", c => c.Long(nullable: false));
            DropColumn("dbo.EntityReview_EntityReview", "RepliedToReviewId");
        }
    }
}
