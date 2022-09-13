using ProductManagement.Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace ProductManagement.Data.EntityConfigurations
{
    public class PropertyEntityTypeConfig : EntityTypeConfiguration<Property>
    {
        public PropertyEntityTypeConfig()
        {
            HasKey(p => p.Id);

            HasOptional(p => p.Category);
        }
    }
}