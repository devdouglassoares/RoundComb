using Core.DynamicProperties.Dtos;
using System;
using System.Collections.Generic;

namespace ProductManagement.Core.Dto
{
    public class VendorProfileModel
	{
		public long Id { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Email { get; set; }

		public string ContactName { get; set; }
		public string ContactPhoneNumber { get; set; }

		public string ProfilePhotoUrl { get; set; }

		public string Description { get; set; }

		public string PhoneNumber { get; set; }

		public long? LocationId { get; set; }

		public string Address { get; set; }

		public string State { get; set; }

		public string City { get; set; }

		public string Country { get; set; }

		public string ZipCode { get; set; }

		public string ModifiedBy { get; set; }

        public long UserId { get; set; }

		public DateTimeOffset? ModifiedDate { get; set; }

		public DynamicPropertyValuesModel ExtendedProperties { get; set; }
		public List<ServiceProvidingDto> ServiceProvidings { get; set; }
	}
}
