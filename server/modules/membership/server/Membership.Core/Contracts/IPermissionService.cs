using System.Linq;
using Core;
using Membership.Core.Dto;

namespace Membership.Core.Contracts
{
    public interface IPermissionService : IDependency
    {
        IQueryable<PermissionModel> GetAll();

        IQueryable<PermissionModel> GetForUser(long userId);

        IQueryable<PermissionModel> GetForRole(long roleId);

        bool AssignToUser(long userId, long permissionId);

        bool AssignToRole(long roleId, long permissionId);

        bool RemoveFromUser(long userId, long permissionId);

        bool RemoveFromRole(long roleId, long permissionId);

        bool IsAssignedToUser(long userId, long permissionId);

        bool IsAssignedToRole(long roleId, long permissionId);

        //IList<string> GetLicensePermissions();

        //IList<ApplicationPermission> GetApplicationPermissions();
    }
}
