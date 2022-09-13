using Core.Templating.Models;
using ProductManagement.Core.Dto;
using Membership.Core.Dto;

namespace ProductManagement.Core.EmailTemplates
{
    public class ApplicationSubmissionFailedNotificationToPropertyOwner : BaseTemplateModel
    {
        public UserPersonalInformation Recipient { get; set; }

        public PropertyDto Property { get; set; }

        public string BaseUrl { get; set; }
    }
}