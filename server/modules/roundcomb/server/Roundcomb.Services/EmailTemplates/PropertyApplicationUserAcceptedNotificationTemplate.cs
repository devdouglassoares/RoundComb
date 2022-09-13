using Core.Templating.Models;

namespace Roundcomb.Services.EmailTemplates
{
    public class PropertyApplicationUserAcceptedNotificationTemplate : BaseTemplateModel
    {
        public long PropertyId { get; set; }

        public string PropertyName { get; set; }

        public string AcceptedDate { get; set; }

        public string ClientName { get; set; }
        public string OwnerName { get; set; }
    }
}