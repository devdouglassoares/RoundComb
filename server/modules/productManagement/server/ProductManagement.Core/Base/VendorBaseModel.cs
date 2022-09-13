using Core.Database.Entities;
using ProductManagement.Core.Entities;
using System.Collections.Generic;

namespace ProductManagement.Core.Base
{
    public abstract class VendorBaseModel : BaseEntity
    {
		public string Name { get; set; }

		public string ContactPhoneNumber { get; set; }
		public string Fax { get; set; }

		public string EmailAddress { get; set; }

		public string Address { get; set; }

		public string City { get; set; }

		public string State { get; set; }

		public string Country { get; set; }

		public string ZipCode { get; set; }

		public string FormattedAddress { get; set; }
		public string ProfilePhotoUrl { get; set; }
		public string Description { get; set; }
		public long UserId { get; set; }
		public virtual ICollection<ServiceProviding> ServiceProvidings { get; set; }
		public long? LocationId { get; set; }
	}
}