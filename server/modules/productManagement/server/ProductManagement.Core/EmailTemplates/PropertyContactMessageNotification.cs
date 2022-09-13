using Core.Templating.Models;
using System;

namespace ProductManagement.Core.EmailTemplates
{
	public class PropertyContactMessageNotification : BaseTemplateModel
	{
		public string ThreadGuid { get; set; }

		public string OwnerName { get; set; }

        public long PropertyId { get; set; }

        public string PropertyDetailBaseUrl { get; set; }

        public string PropertyName { get; set; }

        public string SenderName { get; set; }

        public string Message { get; set; }

        public DateTimeOffset? SentDate { get; set; }
    }
}