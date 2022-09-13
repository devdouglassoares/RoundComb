using Core.Templating.Models;

namespace Membership.Library.Templates
{
    public class UserActivationEmailTemplateModel : BaseTemplateModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ActivationBaseUrl { get; set; }

        public string ActivationCode { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}