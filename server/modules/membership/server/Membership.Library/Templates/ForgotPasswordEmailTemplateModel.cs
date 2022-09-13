using Core.Templating.Models;

namespace Membership.Library.Templates
{
    public class ForgotPasswordEmailTemplateModel : BaseTemplateModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ResetPasswordUrl { get; set; }
        public string ResetPasswordToken { get; set; }
    }
}