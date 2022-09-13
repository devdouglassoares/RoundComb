using ProductManagement.Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace ProductManagement.Data.EntityConfigurations
{
    public class PropertyImageEntityTypeConfig : EntityTypeConfiguration<PropertyImage>
    {
        public PropertyImageEntityTypeConfig()
        {
            HasRequired(image => image.Property)
                .WithMany(p => p.Images);
        }
    }
}