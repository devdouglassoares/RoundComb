using System.Collections.Generic;
using Membership.Core.Entities.Enums;

namespace Membership.Library.Dto
{
    public class CompanySettingsGroupDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Dictionary<CompanySettings, string> Settings { get; set; }
    }
}
