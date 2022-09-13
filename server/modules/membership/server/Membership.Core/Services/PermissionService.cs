using Core.ObjectMapping;
using Membership.Core.Contracts;
using Membership.Core.Data;
using Membership.Core.Dto;
using Membership.Core.Entities;
using Membership.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Membership.Core.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly ICoreRepository _repository;
        private readonly IMappingService _mappingService;

        public PermissionService(ICoreRepository repository, IMappingService mappingService)
        {
            this._repository = repository;
            _mappingService = mappingService;
        }

        public IQueryable<PermissionModel> GetAll()
        {
            var accessEntities = _repository.GetAll<AccessEntity>();

            return _mappingService.Project<AccessEntity, PermissionModel>(accessEntities);
        }

        public IQueryable<PermissionModel> GetForUser(long userId)
        {
            var user = _repository.Get<User>(userId);
            var userAccess = user.UserAccessRights.SelectMany(u => u.AccessModule.AccessEntities);
            var rolesAccess =
                user.Roles.SelectMany(r => r.RoleAccessRights)
                    .SelectMany(r => r.AccessModule.AccessEntities.Select(a => new Tuple<Role, AccessEntity>(r.Role, a)));

            var permissions = new List<PermissionModel>();
            permissions.AddRange(_mappingService.Map<IEnumerable<PermissionModel>>(userAccess));
            permissions.AddRange(_mappingService.Map<IEnumerable<PermissionModel>>(rolesAccess));

            permissions = permissions.Distinct().ToList();
            return permissions.AsQueryable();
        }

        public IQueryable<PermissionModel> GetForRole(long roleId)
        {
            var accessEntities = _repository.GetAll<AccessEntity>()
                                            .Where(a => a.AccessModules.Any(m => m.RoleAccessRights.Any(r => r.Role.Id == roleId)));

            return _mappingService.Project<AccessEntity, PermissionModel>(accessEntities);
        }

        public bool AssignToUser(long userId, long permissionId)
        {
            var user = _repository.Get<User>(userId);
            var accessEntity = _repository.Get<AccessEntity>(permissionId);

            if (user == null || accessEntity == null)
            {
                return false;
            }

            var accessRight = _repository.GetAll<AccessRight>().First(r => r.AccessKind == AccessKind.Write);
            var module = new AccessModule { Name = accessEntity.Name };
            module.AccessEntities.Add(accessEntity);

            var userPerm = new UserAccessRight { User = user, AccessRight = accessRight, AccessModule = module };

            _repository.Insert(module);
            _repository.Insert(userPerm);
            _repository.SaveChanges();

            return true;
        }

        public bool RemoveFromUser(long userId, long permissionId)
        {
            var user = _repository.Get<User>(userId);
            if (user == null)
            {
                return false;
            }

            var rights =
                user.UserAccessRights.Where(u => u.AccessModule.AccessEntities.Any(e => e.Id == permissionId)).ToList();
            foreach (var userAccessRight in rights)
            {
                _repository.Delete(userAccessRight.AccessModule);
                _repository.Delete(userAccessRight);
            }

            if (rights.Any())
            {
                _repository.SaveChanges();
                return true;
            }
            return false;
        }

        public bool AssignToRole(long roleId, long permissionId)
        {
            var role = _repository.Get<Role>(roleId);
            var accessEntity = _repository.Get<AccessEntity>(permissionId);

            if (role == null || accessEntity == null)
            {
                return false;
            }

            var accessRight = _repository.GetAll<AccessRight>().First(r => r.AccessKind == AccessKind.Write);
            var module = new AccessModule { Name = accessEntity.Name };
            module.AccessEntities.Add(accessEntity);

            var rolePerm = new RoleAccessRight { Role = role, AccessRight = accessRight, AccessModule = module };

            _repository.Insert(module);
            _repository.Insert(rolePerm);
            _repository.SaveChanges();

            return true;
        }

        public bool RemoveFromRole(long roleId, long permissionId)
        {
            var user = _repository.Get<Role>(roleId);
            if (user == null)
            {
                return false;
            }

            var rights =
                user.RoleAccessRights.Where(u => u.AccessModule.AccessEntities.Any(e => e.Id == permissionId)).ToList();
            foreach (var roleAccessRight in rights)
            {
                _repository.Delete(roleAccessRight.AccessModule);
                _repository.Delete(roleAccessRight);
            }

            if (rights.Any())
            {
                _repository.SaveChanges();
                return true;
            }
            return false;
        }

        public bool IsAssignedToRole(long roleId, long permissionId)
        {
            var role = _repository.Get<Role>(roleId);
            return role.RoleAccessRights.Any(u => u.AccessModule.AccessEntities.Any(e => e.Id == permissionId));
        }

        public bool IsAssignedToUser(long userId, long permissionId)
        {
            var user = _repository.Get<User>(userId);
            return user.UserAccessRights.Any(u => u.AccessModule.AccessEntities.Any(e => e.Id == permissionId)) ||
                   user.Roles.Any(
                       r =>
                       r.RoleAccessRights.Any(rar => rar.AccessModule.AccessEntities.Any(e => e.Id == permissionId)));
        }
    }
}