using ProductManagement.Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace ProductManagement.Data.EntityConfigurations
{
    public class VendorRequestEntityTypeConfig : EntityTypeConfiguration<VendorRequest>
    {
        public VendorRequestEntityTypeConfig()
        {
            HasKey(r => r.Id);
        }
    }
}
