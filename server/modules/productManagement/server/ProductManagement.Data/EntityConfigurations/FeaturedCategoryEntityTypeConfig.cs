using ProductManagement.Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace ProductManagement.Data.EntityConfigurations
{
    public class FeaturedCategoryEntityTypeConfig : EntityTypeConfiguration<FeaturedCategory>
    {
        public FeaturedCategoryEntityTypeConfig()
        {
            HasKey(x => x.Id);
        }
    }
}