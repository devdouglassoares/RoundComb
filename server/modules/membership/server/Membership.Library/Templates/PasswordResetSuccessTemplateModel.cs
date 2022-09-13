using Core.Templating.Models;

namespace Membership.Library.Templates
{
    public class PasswordResetSuccessTemplateModel : BaseTemplateModel
    {
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PasswordChangedDate { get; set; }
    }
}