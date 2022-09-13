using Core.Templating.Models;

namespace Membership.Library.Templates
{
    public class UserRegistrationApprovedInformTemplateModel : BaseTemplateModel
    {
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ApprovedDate { get; set; }
    }
}