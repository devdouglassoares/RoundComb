using ProductManagement.Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace ProductManagement.Data.EntityConfigurations
{
    public class PropertyDynamicPropertyCategoryEntityTypeConfig : EntityTypeConfiguration<PropertyDynamicPropertyCategory>
    {
        public PropertyDynamicPropertyCategoryEntityTypeConfig()
        {
            HasKey(x => new
            {
                x.CategoryId,
                x.DynamicPropertyId
            });

            HasRequired(x => x.Category)
                .WithMany(y => y.DynamicProperties)
                .HasForeignKey(x => x.CategoryId);
        }
    }
}