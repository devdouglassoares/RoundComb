using ProductManagement.Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace ProductManagement.Data.EntityConfigurations
{
    public class PropertySearchRecordEntityTypeConfig : EntityTypeConfiguration<PropertySearchRecord>
    {
        public PropertySearchRecordEntityTypeConfig()
        {
            HasKey(o => o.Id);
        }
    }
}