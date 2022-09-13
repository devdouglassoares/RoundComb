using Core.DynamicProperties.Models;
using System.Data.Entity.ModelConfiguration;

namespace Core.DynamicProperties.EntityConfigMapping
{
    public class DynamicPropertyEntityTypeConfiguration : EntityTypeConfiguration<DynamicProperty>
    {
        public DynamicPropertyEntityTypeConfiguration()
        {
            HasKey(x => x.Id);

            HasMany(x => x.DynamicPropertyEntityTypeMappings)
                .WithRequired(x => x.DynamicProperty);
        }

        public DynamicPropertyEntityTypeConfiguration(string tableName) : this()
        {
            ToTable(tableName);
        }
    }
}