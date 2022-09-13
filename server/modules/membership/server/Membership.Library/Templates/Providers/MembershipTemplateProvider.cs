using Core.Templating.Services;

namespace Membership.Library.Templates.Providers
{
    public class MembershipTemplateProvider : ITemplateProvider
    {
        public void RegisterTemplates(ITemplateRegistration templateRegistration)
        {
            templateRegistration.RegisterTemplate<ForgotPasswordEmailTemplateModel>();

            templateRegistration.RegisterTemplate<PasswordResetSuccessTemplateModel>();

            templateRegistration.RegisterTemplate<UserActivationEmailTemplateModel>();

            templateRegistration.RegisterTemplate<UserRegistrationApprovalPendingAdminNotificationTemplateModel>();

            templateRegistration.RegisterTemplate<UserRegistrationApprovedInformTemplateModel>();

            templateRegistration.RegisterTemplate<UserRegistrationAdminNotificationTemplateModel>();
        }
    }
}