using System.Collections.Generic;

namespace Membership.Core.Dto
{
    public class RoleModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<PermissionModel> Permissions { get; set; }

        public List<string> Users { get; set; }
        public string Code { get; set; }
    }
}
