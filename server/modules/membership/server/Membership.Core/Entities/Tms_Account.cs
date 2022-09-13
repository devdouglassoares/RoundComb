using Membership.Core.Entities.Base;

namespace Membership.Core.Entities
{
    public class Tms_Account : BaseEntity
    {
        public string JiraUrl { get; set; }
        public string ProjectKey { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
