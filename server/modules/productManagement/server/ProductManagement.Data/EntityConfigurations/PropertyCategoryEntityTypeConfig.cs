using ProductManagement.Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace ProductManagement.Data.EntityConfigurations
{
    public class PropertyCategoryEntityTypeConfig : EntityTypeConfiguration<PropertyCategory>
    {
        public PropertyCategoryEntityTypeConfig()
        {
            HasOptional(category => category.ParentCategory)
                .WithMany(category => category.Children);
        }
    }
}