namespace EntityReviews.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_EntityReviewTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EntityReview_EntityReview",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ReviewerUserId = c.Long(nullable: false),
                        TargetEntityId = c.Long(nullable: false),
                        TargetEntityObject = c.String(),
                        Title = c.String(maxLength: 256),
                        ReviewText = c.String(maxLength: 1000),
                        Rating = c.Int(nullable: false),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                        IsApproved = c.Boolean(nullable: false),
                        ApprovedDate = c.DateTimeOffset(precision: 7),
                        ApprovedBy = c.String(),
                        LikesCount = c.Int(nullable: false),
                        DislikesCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EntityReview_EntityReview");
        }
    }
}
