using System;
using Membership.Core.Entities.Enums;

namespace Membership.Core.Dto
{
    public class PermissionModel : IEquatable<PermissionModel>
    {
        public PermissionModel()
        {
            this.RoleName = string.Empty;
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public AccessEntityType Type { get; set; }

        public string RoleName { get; set; }

        public long? RoleId { get; set; }
        public bool Equals(PermissionModel other)
        {
            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public byte Status { get; set; }
    }
}
