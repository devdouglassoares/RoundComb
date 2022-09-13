using System.Data.Entity.ModelConfiguration;

namespace EntityReviews.EntityMapping
{
    public class EntityRatingEntityTypeConfig : EntityTypeConfiguration<Models.EntityReview>
    {
        public EntityRatingEntityTypeConfig()
        {
            HasKey(x => x.Id);

            HasOptional(review => review.RepliedToReview)
                .WithMany(review => review.Replies);
        }
    }
}