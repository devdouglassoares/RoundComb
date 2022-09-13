using Core.Templating.Services;

namespace ProductManagement.Core.EmailTemplates
{
    public class PropertyManagementTemplateProvider : ITemplateProvider
    {
        public void RegisterTemplates(ITemplateRegistration templateRegistration)
        {
            templateRegistration.RegisterTemplate<PropertyContactMessageNotification>();
            templateRegistration.RegisterTemplate<DirectContactMessageNotification>();
            templateRegistration.RegisterTemplate<ContactMessageRepliedNotification>();

            templateRegistration.RegisterTemplate<PropertyApplicationFormApprovedTemplate>();
            templateRegistration.RegisterTemplate<PropertyApplicationFormRejectedTemplate>();

            templateRegistration.RegisterTemplate<PropertyApplicationFormSubmittedTemplate>();

            templateRegistration.RegisterTemplate<PropertySuggestionEmailTemplate>();

            templateRegistration.RegisterTemplate<ApplicationSubmissionFailedNotificationToPropertyOwner>();

            templateRegistration.RegisterTemplate<SendPropertyToVendorTemplate>();
            templateRegistration.RegisterTemplate<VendorApproveProperty>();
        }
    }
}