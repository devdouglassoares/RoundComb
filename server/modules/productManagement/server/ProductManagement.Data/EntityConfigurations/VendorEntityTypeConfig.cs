using ProductManagement.Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace ProductManagement.Data.EntityConfigurations
{
    public class VendorEntityTypeConfig : EntityTypeConfiguration<Vendor>
    {
        public VendorEntityTypeConfig()
        {
            HasKey(vendor => vendor.Id);
        }
    }
}