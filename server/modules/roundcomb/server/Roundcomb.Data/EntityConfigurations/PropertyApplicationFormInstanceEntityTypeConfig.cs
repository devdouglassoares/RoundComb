using Roundcomb.Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Roundcomb.Data.EntityConfigurations
{
    public class PropertyApplicationFormInstanceEntityTypeConfig : EntityTypeConfiguration<PropertyApplicationFormInstance>
    {
        public PropertyApplicationFormInstanceEntityTypeConfig()
        {
            HasKey(o => o.Id);
        }
    }
}