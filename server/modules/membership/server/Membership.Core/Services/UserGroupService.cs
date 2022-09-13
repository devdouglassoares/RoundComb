using Core.ObjectMapping;
using Membership.Core.Contracts;
using Membership.Core.Data;
using Membership.Core.Dto;
using Membership.Core.Entities;
using System.Linq;

namespace Membership.Core.Services
{
    public class UserGroupService : IUserGroupService
    {
        private readonly IMappingService _mappingService;
        private readonly ICoreRepository _repository;

        public UserGroupService(ICoreRepository repository, IMappingService mappingService)
        {
            _repository = repository;
            _mappingService = mappingService;
        }

        public IQueryable<GroupModel> QueryGroups()
        {
            return _mappingService.Project<Group, GroupModel>(_repository.GetAll<Group>());
        }

        public GroupModel Save(GroupModel model)
        {
            Group group;

            if (model.Id == 0)
            {
                group = _mappingService.Map<Group>(model);
            }
            else
            {
                group = _repository.Get<Group>(model.Id);
                if (group == null)
                {
                    return null;
                }

                _mappingService.Map(model, group);
            }

            var userIds = model.Users.Select(x => x.Id).Distinct();
            var users = _repository.GetAll<User>().Where(x => userIds.Contains(x.Id)).ToList();

            group.Users.Clear();
            foreach (var user in users)
            {
                group.Users.Add(user);
            }

            if (model.Id == 0)
            {
                _repository.Insert(group);
            }
            else
            {
                _repository.Update(group);
            }

            _repository.SaveChanges();

            return _mappingService.Map<GroupModel>(group);
        }

        public bool Delete(long id)
        {
            var group = _repository.Get<Group>(id);

            if (group == null)
            {
                return false;
            }

            _repository.Delete(group);
            _repository.SaveChanges();

            return true;
        }
    }
}