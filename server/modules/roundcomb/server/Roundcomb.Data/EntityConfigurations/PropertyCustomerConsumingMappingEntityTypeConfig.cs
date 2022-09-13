using Roundcomb.Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Roundcomb.Data.EntityConfigurations
{
    public class PropertyCustomerConsumingMappingEntityTypeConfig : EntityTypeConfiguration<PropertyCustomerConsumingMapping>
    {
        public PropertyCustomerConsumingMappingEntityTypeConfig()
        {
            HasKey(o => o.Id);
        }
    }
}