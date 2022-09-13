using Membership.Core.Entities.Base;

namespace Membership.Core.Entities
{
    public class Contact : BaseEntity
    {
        public string Type { get; set; }

        public string Value { get; set; }

        public long UserId { get; set; }

        public virtual User User { get; set; }
    }
}
