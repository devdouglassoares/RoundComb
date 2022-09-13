using Core.DynamicProperties.Models;
using System.Data.Entity.ModelConfiguration;

namespace Core.DynamicProperties.EntityConfigMapping
{
    public class DynamicPropertyEntityTypeMappingEntityTypeConfig : EntityTypeConfiguration<DynamicPropertyEntityTypeMapping>
    {
        public DynamicPropertyEntityTypeMappingEntityTypeConfig()
        {
            HasKey(x => new
            {
                x.DynamicPropertyId,
                x.TargetEntityType
            });
        }
    }
}