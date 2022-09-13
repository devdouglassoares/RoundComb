using Core.Templating.Models;

namespace Roundcomb.Services.EmailTemplates
{
    public class PropertyApplicationUserDeclinedNotificationTemplate : BaseTemplateModel
    {
        public long PropertyId { get; set; }

        public string PropertyName { get; set; }

        public string DeclinedDate { get; set; }

        public string DeclineReason { get; set; }

        public string ClientName { get; set; }

        public string OwnerName { get; set; }
    }
}