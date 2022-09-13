using System.Linq;
using Core;
using Membership.Core.Dto;

namespace Membership.Core.Contracts
{
    public interface IUserGroupService : IDependency
    {
        IQueryable<GroupModel> QueryGroups();

        GroupModel Save(GroupModel model);

        /*GroupModel Add(GroupModel model);

        bool Update(GroupModel model);*/

        bool Delete(long id);
    }
}
