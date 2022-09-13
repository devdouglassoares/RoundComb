using System.Collections.Generic;
using Membership.Core.Entities.Base;
using Membership.Core.Entities.Enums;

namespace Membership.Core.Entities
{
    public class CompanySetting : BaseEntity
    {
        public virtual Company Company { get; set; }
        public CompanySettings Type { get; set; }
        public string Value { get; set; }
        public virtual CompanySetting Parent { get; set; }
        public virtual ICollection<CompanySetting> SubSettings { get; set; }
    }
}
