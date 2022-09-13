using Core.ObjectMapping;
using Membership.Core.Contracts;
using Membership.Core.Data;
using Membership.Core.Dto;
using Membership.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Membership.Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly IMappingService _mappingService;
        private readonly ICoreRepository _repository;

        public RoleService(ICoreRepository repository, IMappingService mappingService)
        {
            _repository = repository;
            _mappingService = mappingService;
        }

        public IQueryable<RoleModel> GetAll()
        {
            var roles = _repository.GetAll<Role>();

            return _mappingService.Project<Role, RoleModel>(roles);
        }

        public IQueryable<Role> AllQueriable()
        {
            return _repository.GetAll<Role>();
        }

        public RoleModel Get(long id)
        {
            var role = _repository.Get<Role>(id);
            if (role == null)
            {
                return null;
            }

            var model = _mappingService.Map<RoleModel>(role);
            //model.Users = role.Users.Select(u => string.Format("{0} {1}", u.FirstName, u.LastName)).ToList();
            return model;
        }

        public bool Update(RoleModel model)
        {
            var role = _repository.Get<Role>(model.Id);
            if (role == null)
            {
                return false;
            }

            _mappingService.Map(model, role);
            _repository.Update(role);
            _repository.SaveChanges();

            return true;
        }

        public RoleModel Add(RoleModel model)
        {
            var role = _mappingService.Map<Role>(model);

            _repository.Insert(role);
            _repository.SaveChanges();

            return _mappingService.Map<RoleModel>(role);
        }

        public bool Delete(long id)
        {
            var role = _repository.Get<Role>(id);

            if (role == null)
            {
                return false;
            }

            _repository.Delete(role);
            _repository.SaveChanges();

            return true;
        }

        public IList<RoleModel> GetForUser(long userId)
        {
            var user = _repository.Get<User>(userId);

            return _mappingService.Map<IList<RoleModel>>(user.Roles);
        }

        /*public bool AssignToUser(long userId, long roleId)
        {
            var user = this.repository.Get<User>(userId);
            var role = this.repository.Get<Role>(roleId);

            if (user == null || role == null)
            {
                return false;
            }

            user.Roles.Add(role);
            if (role.Code == MembershipConstant.C_ROLE_CODE_COMPANYADMIN)
            {
                var folder = new Folder() {Name = "Root", ClientPath = string.Empty};
                var computer = new Computer() { RootFolder = folder };
                var bo = new Company{Domain = user.Email.Replace("@", "_").Replace(".", "_"), Owner = user, Computer = computer};
                repository.Insert(folder);
                repository.Insert(computer);
                repository.Insert(bo);
            }

            this.repository.Update(user);
            this.repository.SaveChanges();
            return true;
        }*/

        public bool AssignToUser(long userId, long roleId)
        {
            var user = _repository.Get<User>(userId);
            var role = _repository.Get<Role>(roleId);

            if (user == null || role == null)
            {
                return false;
            }

            user.Roles.Add(role);

            _repository.Update(user);
            _repository.SaveChanges();
            return true;
        }

        public bool RemoveFromUser(long userId, long roleId)
        {
            var user = _repository.Get<User>(userId);

            var role = user?.Roles.FirstOrDefault(r => r.Id == roleId);
            if (role == null)
            {
                return false;
            }

            user.Roles.Remove(role);

            _repository.Update(user);
            _repository.SaveChanges();
            return true;
        }

        public bool IsAssignedTo(long userId, long roleId)
        {
            return _repository.GetAll<User>().Any(u => u.Id == userId && u.Roles.Any(r => r.Id == roleId));
        }

        public IList<string> GetSystemRolesCodes()
        {
            return new List<string>
                   {
                       MembershipConstant.C_ROLE_CODE_COMPANYCLIENT,
                       MembershipConstant.C_ROLE_CODE_COMPANYADMIN,
                       MembershipConstant.C_ROLE_CODE_COMPANYSTAFF,
                       MembershipConstant.C_ROLE_CODE_COMPANYCLIENTSTAFF
                   };
        }
    }
}