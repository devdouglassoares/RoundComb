using Core.Database.Entities;
using ProductManagement.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Roundcomb.Core.Base
{
	public abstract class PropertyCustomerConsumingMappingBase : BaseEntity
	{
		public long PropertyId { get; set; }

		/// <summary>
		/// Get or set the identifier of the customer who consumes the product
		/// </summary>
		public long CustomerId { get; set; }

		public long PropertyApplicationFormInstanceId { get; set; }

		[NotMapped]
		public PropertyAssignment Assignment { get; set; }

		public DateTimeOffset? StartDate { get; set; }

		public int? ValidLeaseDurationInMonth { get; set; }

		public bool IsCompleted { get; set; }

		public bool IsRenewable { get; set; }

		public bool NotifyCustomerBeforeExpire { get; set; }
	}
}