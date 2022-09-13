using System.Data.Entity.ModelConfiguration;
using Core.DynamicProperties.Models;

namespace Core.DynamicProperties.EntityConfigMapping
{
    public class DynamicPropertySupportedEntityTypesEntityTypeConfig : EntityTypeConfiguration<DynamicPropertySupportedEntityType>
    {
        public DynamicPropertySupportedEntityTypesEntityTypeConfig()
        {
            HasKey(x => x.EntityTypeFullName);
        }
    }
}