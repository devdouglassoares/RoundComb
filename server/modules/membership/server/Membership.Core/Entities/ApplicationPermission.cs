using Membership.Core.Entities.Base;

namespace Membership.Core.Entities
{
    public class ApplicationPermission : BaseEntity
    {
        public virtual string Key { get; set; }

        public virtual string Value { get; set; }
        public virtual bool IsEnabled { get; set; }
    }
}
