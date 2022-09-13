using Core.DynamicProperties.Models;
using System.Data.Entity.ModelConfiguration;

namespace Core.DynamicProperties.EntityConfigMapping
{
    public class DynamicPropertyValueEntityTypeConfiguration : EntityTypeConfiguration<DynamicPropertyValue>
    {
        public DynamicPropertyValueEntityTypeConfiguration()
        {
            HasKey(x => x.Id);
        }

        public DynamicPropertyValueEntityTypeConfiguration(string tableName) : this()
        {
            ToTable(tableName);
        }
    }
}