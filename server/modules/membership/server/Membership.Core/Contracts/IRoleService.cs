using System.Collections.Generic;
using System.Linq;
using Core;
using Membership.Core.Dto;
using Membership.Core.Entities;

namespace Membership.Core.Contracts
{
    public interface IRoleService : IDependency
    {
        IQueryable<RoleModel> GetAll();

        IQueryable<Role> AllQueriable();

        RoleModel Get(long id);

        bool Update(RoleModel model);

        RoleModel Add(RoleModel model);

        bool Delete(long id);

        IList<RoleModel> GetForUser(long userId);

        bool AssignToUser(long userId, long roleId);

        bool RemoveFromUser(long userId, long roleId);

        bool IsAssignedTo(long userId, long roleId);
        IList<string> GetSystemRolesCodes();
    }
}
