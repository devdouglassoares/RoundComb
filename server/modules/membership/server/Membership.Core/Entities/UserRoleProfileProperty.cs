using System.ComponentModel.DataAnnotations.Schema;

namespace Membership.Core.Entities
{
    public class UserRoleProfileProperty
    {
        public long PropertyId { get; set; }

        public long UserRoleId { get; set; }

        [ForeignKey("UserRoleId")]
        public virtual Role UserRole { get; set; }
    }
}