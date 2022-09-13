namespace Membership.Core.Entities.Base
{
    /// <summary>
    ///     The permission entity.
    /// </summary>
    public abstract class PermissionEntity : BaseEntity
    {
        public virtual AccessEntity AccessEntity { get; set; }
    }
}