using Membership.Core.Entities.Base;

namespace Membership.Core.Entities
{
    public class UserExternalLogin : BaseEntity
    {
        public virtual User User { get; set; }
        public string ExternalProviderName { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
    }
}