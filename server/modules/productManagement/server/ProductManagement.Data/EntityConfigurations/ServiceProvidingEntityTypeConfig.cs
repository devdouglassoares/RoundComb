using ProductManagement.Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace ProductManagement.Data.EntityConfigurations
{
	public class ServiceProvidingEntityTypeConfig : EntityTypeConfiguration<ServiceProviding>
	{
		public ServiceProvidingEntityTypeConfig()
		{
			HasKey(r => r.Id);
			//.HasMany<Vendor>(s => s.Vendors)
			// .WithMany(c => c.ServiceProvidings)
			// .Map(cs =>
			//	  {
			//		  cs.MapLeftKey("ServiceProvidingId");
			//		  cs.MapRightKey("VendorId");
			//	  });
		}
	}
}
