using Core.Templating.Models;

namespace Membership.Library.Templates
{
    public class UserRegistrationApprovalPendingAdminNotificationTemplateModel : BaseTemplateModel
    {
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string HomePhoneNumber { get; set; }

        public string RegistrationDate { get; set; }
    }
}