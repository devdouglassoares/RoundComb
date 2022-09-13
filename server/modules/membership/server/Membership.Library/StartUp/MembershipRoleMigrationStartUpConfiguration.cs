using Core.StartUp;
using Membership.Core;
using Membership.Core.Contracts;
using Membership.Core.Entities;
using Membership.Library.Data;
using System.Collections.Generic;
using System.Linq;

namespace Membership.Library.StartUp
{
    public class MembershipRoleMigrationStartUpConfiguration : IApplicationStartUpExecution
    {
        private readonly IRepository _repository;

        private readonly IPermissionService _permissionService;

        private readonly Dictionary<string, string> _roleMigrations = new Dictionary<string, string>
                                                                     {
                                                                         {"BO", MembershipConstant.C_ROLE_CODE_COMPANYADMIN}, // BizOwner => CompanyAdmin,
                                                                         {"BS", MembershipConstant.C_ROLE_CODE_COMPANYSTAFF}, // BizStaff => CompanyStaff,
                                                                         {"BC", MembershipConstant.C_ROLE_CODE_COMPANYCLIENT}, // BizClient => CompanyClient,

                                                                     };

        private readonly List<Role> _roles = new List<Role>
            {
                new Role
                {
                    Name = MembershipConstant.C_ROLE_NAME_SYSADMIN,
                    Code = MembershipConstant.C_ROLE_CODE_SYSADMIN,
                    Description = MembershipConstant.C_ROLE_NAME_SYSADMIN
				},
                new Role
                {
                    Name = MembershipConstant.C_ROLE_NAME_COMPANYADMIN,
                    Code = MembershipConstant.C_ROLE_CODE_COMPANYADMIN,
                    Description = MembershipConstant.C_ROLE_NAME_COMPANYADMIN
                },
                new Role
                {
                    Name = MembershipConstant.C_ROLE_NAME_COMPANYSTAFF,
                    Code = MembershipConstant.C_ROLE_CODE_COMPANYSTAFF,
                    Description = MembershipConstant.C_ROLE_NAME_COMPANYSTAFF
                },
                new Role
                {
                    Name = MembershipConstant.C_ROLE_NAME_COMPANYCLIENT,
                    Code = MembershipConstant.C_ROLE_CODE_COMPANYCLIENT,
                    Description = MembershipConstant.C_ROLE_NAME_COMPANYCLIENT
                },
                new Role
                {
                    Name = MembershipConstant.C_ROLE_NAME_COMPANYCLIENTSTAFF,
                    Code = MembershipConstant.C_ROLE_CODE_COMPANYCLIENTSTAFF,
                    Description = MembershipConstant.C_ROLE_NAME_COMPANYCLIENTSTAFF
                }
            };

        public MembershipRoleMigrationStartUpConfiguration(IRepository repository, IPermissionService permissionService)
        {
            _repository = repository;
            _permissionService = permissionService;
        }

        public bool ShouldRun()
        {
            var keys = _roleMigrations.Keys;

            // only run if any old role still exists
            var shouldRun = _repository.Any<Role>(x => keys.Contains(x.Code));
            if (shouldRun) return true;

            var existingRoleCodes = _repository.GetAll<Role>().Select(x => x.Code);

            return _roles.Any(role => !existingRoleCodes.Contains(role.Code));
        }

        public void Execute()
        {
            InsertRolesIfNotExist();

            MigrateUserRoles();

            MigrateRolePermissions();

            DeleteOldRoles();
        }

        private void InsertRolesIfNotExist()
        {
            foreach (var item in _roles)
            {
                if (!_repository.Any<Role>(x => x.Name == item.Name))
                {
                    item.IsActive = true;
                    _repository.Insert(item);
                }
            }
            _repository.SaveChanges();
        }

        private void MigrateUserRoles()
        {
            var keys = _roleMigrations.Keys;

            foreach (var oldRole in keys)
            {
                var oldRoleEntity = _repository.Get<Role>(x => x.Code == oldRole);
                var newRole = _roleMigrations[oldRole];
                var newRoleEntity = _repository.Get<Role>(x => x.Code == newRole);

                if (oldRoleEntity == null || newRoleEntity == null) continue;

                _repository.DbContext.Database.ExecuteSqlCommand(
                    $"UPDATE RoleUser SET RoleId={newRoleEntity.Id} WHERE RoleId={oldRoleEntity.Id}");
            }
            _repository.SaveChanges();
        }

        private void MigrateRolePermissions()
        {
            var keys = _roleMigrations.Keys;

            foreach (var oldRole in keys)
            {
                var oldRoleEntity = _repository.Get<Role>(x => x.Code == oldRole);
                var newRole = _roleMigrations[oldRole];
                var newRoleEntity = _repository.Get<Role>(x => x.Code == newRole);

                if (oldRoleEntity == null || newRoleEntity == null) continue;

                var permissionIds =
                    oldRoleEntity.RoleAccessRights.SelectMany(x => x.AccessModule.AccessEntities.Select(accessEntity => accessEntity.Id));

                foreach (var permissionId in permissionIds)
                {
                    _permissionService.AssignToRole(newRoleEntity.Id, permissionId);
                    _permissionService.RemoveFromRole(oldRoleEntity.Id, permissionId);
                }
            }
        }

        private void DeleteOldRoles()
        {
            var keys = _roleMigrations.Keys;
            _repository.DeleteByCondition<Role>(x => keys.Contains(x.Code));
            _repository.SaveChanges();
        }
    }
}