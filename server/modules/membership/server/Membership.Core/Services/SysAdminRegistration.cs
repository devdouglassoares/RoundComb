using System.Collections.Generic;
using Membership.Core.Contracts;
using Membership.Core.Data;
using Membership.Core.Entities;
using Membership.Core.Implementation;

namespace Membership.Core.Services
{
	public class SysAdminRegistration : IPermissionRegistration
	{
		private readonly ICoreRepository _repository;

		public SysAdminRegistration(ICoreRepository repository)
		{
			_repository = repository;
		}

		public bool ShouldRun()
		{
			var sysAdminRoleExist = _repository.Any<Role>(role => role.Code == MembershipConstant.C_ROLE_CODE_SYSADMIN);
			if (!sysAdminRoleExist)
				return true;

			var sysadminUserExist = _repository.Get<User>(user => user.Email == MembershipConstant.SystemAdminUserName);
			return sysadminUserExist == null;
		}

		public void Execute()
		{
			var systemAdminRole = _repository.Get<Role>(role => role.Code == MembershipConstant.C_ROLE_CODE_SYSADMIN);
			if (systemAdminRole == null)
			{
				systemAdminRole = new Role
				                  {
					                  Code = MembershipConstant.C_ROLE_CODE_SYSADMIN,
					                  Name = MembershipConstant.C_ROLE_NAME_SYSADMIN,
					                  Description = MembershipConstant.C_ROLE_NAME_SYSADMIN
				                  };

				_repository.Insert(systemAdminRole);
			}
			var systemAdminAccount = new User
			                         {
				                         FirstName = "System Admin",
				                         Email = MembershipConstant.SystemAdminUserName,
				                         PasswordHash = MD5HashCrypto.GetHash("qwerty"),
				                         IsActive = true,
				                         IsApproved = true,
				                         CompanyId = null,
				                         Roles = new List<Role>
				                                 {
					                                 systemAdminRole
				                                 }
			                         };

			_repository.Insert(systemAdminAccount);
			_repository.SaveChanges();
		}
	}
}