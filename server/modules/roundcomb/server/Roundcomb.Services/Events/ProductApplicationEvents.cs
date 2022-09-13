using Roundcomb.Core.Entities;

namespace Roundcomb.Services.Events
{
    public class PropertyApplicationApproved
    {
        public PropertyApplicationFormInstance ApplicationForm { get; set; }
    }

    public class PropertyApplicationUserAccepted
    {
        public PropertyApplicationFormInstance ApplicationForm { get; set; }
    }

    public class PropertyApplicationUserDeclined
    {
        public PropertyApplicationFormInstance ApplicationForm { get; set; }
    }

    public class PropertyApplicationRejected
    {
        public PropertyApplicationFormInstance ApplicationForm { get; set; }
    }

    public class PropertyApplicationSubmitted
    {
        public PropertyApplicationFormInstance ApplicationForm { get; set; }
    }
}