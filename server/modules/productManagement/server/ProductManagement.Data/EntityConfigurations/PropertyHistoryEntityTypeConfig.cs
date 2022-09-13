using ProductManagement.Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace ProductManagement.Data.EntityConfigurations
{
    public class PropertyHistoryEntityTypeConfig : EntityTypeConfiguration<PropertyHistory>
    {
        public PropertyHistoryEntityTypeConfig()
        {
            HasKey(o => o.Id);
        }
    }
}