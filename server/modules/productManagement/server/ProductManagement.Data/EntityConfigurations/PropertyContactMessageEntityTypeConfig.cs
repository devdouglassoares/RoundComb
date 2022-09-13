using ProductManagement.Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace ProductManagement.Data.EntityConfigurations
{
    public class PropertyContactMessageEntityTypeConfig : EntityTypeConfiguration<PropertyContactMessage>
    {
        public PropertyContactMessageEntityTypeConfig()
        {
            HasKey(m => m.Id);

            HasOptional(m => m.Property);
        }
    }
}