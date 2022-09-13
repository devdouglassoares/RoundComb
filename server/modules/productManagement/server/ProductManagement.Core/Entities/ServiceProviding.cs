using System.Collections.Generic;

namespace ProductManagement.Core.Entities
{
	public class ServiceProviding
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public virtual ICollection<Vendor> Vendors { get; set; }
	}
}
