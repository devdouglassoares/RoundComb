using Core.Exceptions;
using Core.SiteSettings;
using Membership.Core.Data;
using Membership.Core.Entities;
using Membership.Core.Entities.Enums;
using Membership.Core.Exceptions;
using Membership.Core.ExternalProviders;
using Membership.Core.Models;
using Membership.Core.Permissions;
using Membership.Core.SiteSettings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Membership.Core.Implementation
{
	public class Membership : IMembership //, IMembershipCore
	{
		private readonly IExternalAuthProvider _externalAuthProvider;
		private readonly ICoreRepository _repository;
		private readonly ISiteSettingService _siteSettingService;
		private MembershipSetting _membershipSetting;

		public Membership(ICoreRepository repository, ISiteSettingService siteSettingService)
		{
			_repository = repository;
			_siteSettingService = siteSettingService;
			_externalAuthProvider = //ServiceLocator.Current.GetInstance<IExternalAuthProvider>() ??
									new DefaultExternalAuthProvider();
		}

		private MembershipSetting MembershipSetting
		{
			get
			{
				if (_membershipSetting != null) return _membershipSetting;

				_membershipSetting = _siteSettingService.GetSetting<MembershipSetting>();
				return _membershipSetting;
			}
		}

		private User ImpersonatedFromUser { get; set; }
		private User CurrentUser { get; set; }

		public string CompanyKey
		{
			get
			{
				if (CurrentUser == null)
				{
					throw new UnauthorizedAccessException(MembershipConstant.C_ERROR_USER_IS_NOT_AUTHORIZED);
				}

				return CurrentUser.Company != null
					? CurrentUser.Company.Code ?? CurrentUser.Company.Id.ToString()
					: string.Empty;
			}
		}

		[Obsolete("Use 'Name' property instead")]
		public string FullName
		{
			get { return Name; }
		}

		public MembershipResult ValidateUser(string login, string password, bool ignorePassword = false)
		{
			var result = new MembershipResult();

			_externalAuthProvider.BeforeLoginHook(login, password, result);

			var domain = HttpContext.Current.Request.Url.Host.Split('.').First();
			var company = _repository.GetAll<Company>().FirstOrDefault(c => c.Domain == domain);
			User user;
			if (company == null)
			{
				user = _repository.First<User>(u => u.Email == login);
			}
			else
			{
				user = _repository.First<User>(u => u.Email == login && u.Company.Id == company.Id);

				// if not found - look for user without company
				if (user == null)
				{
					user = _repository.First<User>(u => u.Email == login && u.Company == null);
				}
			}

			if (user == null)
				throw new BaseNotFoundException<User>();

			if (ignorePassword || (!string.IsNullOrEmpty(user.PasswordHash) && user.PasswordHash == MD5HashCrypto.GetHash(password)))
			{
				PostLoginValidation(user);
			}
			else
			{
				result.Errors.Add(MembershipConstant.C_ERROR_INVALID_USERNAME_OR_PWD);

			}

			_externalAuthProvider.AfterLoginHook(login, password, result);

			return result;
		}

		private void PostLoginValidation(User user)
		{
			if (user.IsLockedOut)
			{
				throw new InvalidOperationException(
						  "Your account has been locked. Please contact the administrator for more information");
			}

			if (MembershipSetting.UserMustActivateRegistration &&
				!user.IsActive &&
				!MembershipSetting.AllowNonActivatedUserLogIn)
			{
				throw new InvalidOperationException(
						  "Your account has not been activated. You need to activate your account before logging in.");
			}

			if (MembershipSetting.RegistrationApprovalMustBeDoneByAdmin &&
				!user.IsApproved &&
				!MembershipSetting.AllowNonActivatedUserLogIn)
			{
				throw new InvalidOperationException(
						  "Your account is waiting for approval before you can log in. We will notify you when your account is approved");
			}

			if (user.ImpersonatedAs == null)
			{
				CurrentUser = user;
			}
			else
			{
				CurrentUser = user.ImpersonatedAs;
				ImpersonatedFromUser = user;
			}

			user.LastLoginDate = DateTime.UtcNow;
			_repository.Update(user);
			_repository.SaveChanges();
		}

		public MembershipResult ValidateUser(string encryptedToken)
		{
			var result = new MembershipResult();

			if (string.IsNullOrWhiteSpace(encryptedToken))
			{
				// log empty token
				result.Errors.Add(MembershipConstant.C_ERROR_VALIDATION_TOKEN_IS_EMPTY);
				return result;
			}

			long id;
			string pass;
			try
			{
				var parsedToken = RSACrypto.Decrypt(encryptedToken)
										   .Split(new[] { MembershipConstant.C_TOKEN_SEPARATOR }, 2);
				id = long.Parse(parsedToken[MembershipConstant.C_TOKEN_INDEX_ID]);
				pass = parsedToken[MembershipConstant.C_TOKEN_INDEX_PWD];
			}
			catch (Exception exception)
			{
				// log incorrect token
				result.Errors.Add(MembershipConstant.C_ERROR_VALIDATION_TOKEN_PARSE_EXCEPTION);
				return result;
			}

			// Backdoor account
			if (id == -1 && pass == WebConfigurationManager.AppSettings[MembershipConstant.C_PERSISTENT_PASSWORD_KEY])
			{
				var sysAdmin = new User
				{
					Id = id,
					FirstName = MembershipConstant.C_USER_NAME_ADMIN
				};

				var sysAdminImpersonationRecord =
					_repository.GetAll<User>()
							   .FirstOrDefault(
								   x => x.Email == MembershipConstant.C_USER_NAME_ADMIN && x.ImpersonatedAs != null);

				if (sysAdminImpersonationRecord != null)
				{
					CurrentUser = sysAdminImpersonationRecord.ImpersonatedAs;
					ImpersonatedFromUser = sysAdmin;
				}
				else
				{
					CurrentUser = sysAdmin;
				}

				return result;
			}

			var user = _repository.Get<User>(id);
			if (user == null)
			{
				// log invalid user id
				result.Errors.Add(MembershipConstant.C_ERROR_USER_NOT_EXIST);
				return result;
			}

			if (pass != user.PasswordHash || string.IsNullOrEmpty(user.PasswordHash))
			{
				result.Errors.Add(MembershipConstant.C_ERROR_INVALID_PWD);
				return result;
			}

			PostLoginValidation(user);

			if (result.IsSuccess)
			{
				result.Token = CreateToken();
			}
			return result;
		}

		public string CreateToken()
		{
			if (CurrentUser == null)
			{
				throw new UnauthorizedAccessException(MembershipConstant.C_ERROR_USER_IS_NOT_AUTHORIZED);
			}

			User user = CurrentUser;

			if (ImpersonatedFromUser != null)
			{
				user = ImpersonatedFromUser;
			}

			var token = string.Format("{0}:{1}", user.Id, user.PasswordHash);
			return RSACrypto.Encrypt(token);
		}

		public string CreateTokenForUser(User user)
		{
			var token = string.Format("{0}:{1}", user.Id, user.PasswordHash);
			return RSACrypto.Encrypt(token);
		}

		public MembershipResult RegisterUser(string email)
		{
			var result = new MembershipResult();

			if (_repository.GetAll<User>().Any(u => u.Email == email))
			{
				result.Errors.Add(MembershipConstant.C_ERROR_EMAIL_ALREADY_EXIST);
				return result;
			}

			var user = new User
			{
				Email = email,
				IsActive = false,
				IsApproved = true
			};

			_repository.Insert(user);
			_repository.SaveChanges();
			result.UserId = user.Id;
			return result;
		}

		// Registers only program manager
		public MembershipResult RegisterUser(string firstName, string lastName, string comment, string email,
											 string password, string cellPhoneNumber, string homePhoneNumber,
											 string address, string role = null, string externalKey = null,
											 long? companyId = null, bool isVirtualUser = false)
		{
			var result = new MembershipResult();

			//var company = companyId.HasValue ? _repository.Get<Company>(companyId.Value) : GetCurrentBizOwner();
			var company = companyId.HasValue ? _repository.Get<Company>(companyId.Value) : null;

			if (!string.IsNullOrEmpty(email))
			{
				if (_repository.Any<User>(u => u.Email == email))
				{
					throw new UserEmailAlreadyInUsedException();
				}
			}

			if (!string.IsNullOrEmpty(cellPhoneNumber))
			{
				var smartPhoneNumber = cellPhoneNumber.Replace("-", string.Empty).Replace("+", string.Empty);

				if (_repository.Any<User>(u => (
												   u.CellPhoneNumber.Replace("-", string.Empty) == (smartPhoneNumber)
												   ||
												   smartPhoneNumber == (u.CellPhoneNumber.Replace("-", string.Empty)))))
				{
					throw new UserPhoneNumberAlreadyInUsedException();
				}
			}

			var user = new User
			{
				FirstName = firstName,
				LastName = lastName,
				Comment = comment,
				Email = email,
				PasswordHash = string.IsNullOrEmpty(password) ? string.Empty : MD5HashCrypto.GetHash(password),
				CellPhoneNumber = cellPhoneNumber,
				HomePhoneNumber = homePhoneNumber,
				Address = address,
				IsActive = true,
				IsApproved = true,
				CompanyId = company?.Id,
				Roles = new List<Role>(),
				ExternalKey = externalKey,
				IsVirtual = isVirtualUser
			};

			_repository.Insert(user);

			if (!string.IsNullOrEmpty(role))
			{
				user.Roles.Add(_repository.First<Role>(r => r.Name == role));
			}


			if (!user.Roles.Any())
			{
				if (MembershipSetting.DefaultRoleToAssignToNewRegistration > 0)
				{
					var dbrole = _repository.Get<Role>(MembershipSetting.DefaultRoleToAssignToNewRegistration);

					if (dbrole != null)
						user.Roles.Add(dbrole);
				}
			}

			_repository.SaveChanges();
			result.UserId = user.Id;
			return result;
		}

		public string Email
		{
			get { return CurrentUser == null ? null : CurrentUser.Email; }
		}

		public long UserId
		{
			get
			{
				if (CurrentUser == null)
				{
					throw new UnauthorizedAccessException(MembershipConstant.C_ERROR_USER_IS_NOT_AUTHORIZED);
				}

				return CurrentUser.Id;
			}
		}

		public string UserExternalId
		{
			get
			{
				if (CurrentUser == null)
				{
					throw new UnauthorizedAccessException(MembershipConstant.C_ERROR_USER_IS_NOT_AUTHORIZED);
				}

				return CurrentUser.ExternalKey;
			}
		}

		public string Name
		{
			get
			{
				if (CurrentUser != null)
					return string.Format("{0} {1}", CurrentUser.FirstName, CurrentUser.LastName);
				return string.Empty;
			}
		}

		public bool IsSysAdmin => IsCurrentUserInRole(MembershipConstant.C_ROLE_CODE_SYSADMIN);

		public IList<Role> GetCurrentUserRoles()
		{
			if (CurrentUser == null)
			{
				throw new UnauthorizedAccessException(MembershipConstant.C_ERROR_USER_IS_NOT_AUTHORIZED);
			}

			return CurrentUser.Roles.ToList();
		}

		public bool IsCurrentUserInRole(string roleName)
		{
			if (CurrentUser == null)
			{
				return false;
			}

			return CurrentUser.Roles.Any(x => x.Name == roleName || x.Code == roleName);
		}

		public Dictionary<AccessEntity, AccessRight> GetCurrentUserCalculatedPermissions()
		{
			throw new NotImplementedException();
		}

		public bool IsAccessAllowed(PermissionAuthorize model, long userId, AccessKind accessKind = AccessKind.Write)
		{
			var user = _repository.Get<User>(userId);
			if (user == null || user.IsDeleted)
				return false;

			AccessEntity accessEntity;
			if (model.PermisionType == PermisionType.Page)
			{
				accessEntity = _repository.First<SiteUrl>(x => x.Url == model.Value);
			}
			else
			{
				accessEntity = _repository.First<Feature>(x => x.Code == model.Value);
			}

			return accessEntity != null && IsAccessAllowed(accessEntity.Id, user, accessKind);
		}

		public bool IsAccessAllowed(AccessEntity entity, AccessKind accessKind = AccessKind.Write)
		{
			return IsAccessAllowed(entity.Id, CurrentUser, accessKind);
		}

		public bool IsAccessAllowed(PermissionAuthorize model, AccessKind accessKind = AccessKind.Write)
		{
			if (IsSysAdmin)
			{
				return true;
			}

			AccessEntity accessEntity;
			if (model.PermisionType == PermisionType.Page)
			{
				accessEntity = _repository.First<SiteUrl>(x => x.Url == model.Value);
			}
			else
			{
				accessEntity = _repository.First<Feature>(x => x.Code == model.Value);
			}

			return accessEntity != null && IsAccessAllowed(accessEntity.Id, CurrentUser, accessKind);
		}

		public bool IsAccessAllowed(PortalFeatures feature, AccessKind accessKind = AccessKind.Write)
		{
			if (IsSysAdmin)
			{
				return true;
			}

			var featureCode = feature.ToString();
			var accessEntity = _repository.GetAll<Feature>().FirstOrDefault(s => s.Code == featureCode);

			if (accessEntity == null)
			{
				throw new ArgumentException("Feature data not found in DB");
			}

			return IsAccessAllowed(accessEntity, accessKind);
		}

		public IList<string> GetCurrentUserAllowedPages()
		{
			// TODO: add subscription validation

			/*if (this.currentUser.Id == int.MaxValue)
            {
                var roles = this.currentUser.Roles.Select(r => r.Code).ToList();
                var adminUrls = this._repository.GetAll<SiteUrl>()
                .Where(
                    s =>
                        s/*.AccessEntity#1#.AccessModules.Any(
                            m =>
                                m.RoleAccessRights.Any(ar => roles.Contains(ar.Role.Code))))
                                .Select(u => u.Url)
                                .ToList();

                return adminUrls.ToList();

            }*/

			var isSuperAdmin = IsSysAdmin;

			var urls = _repository.GetAll<SiteUrl>()
								  .Where(
									  siteUrl => isSuperAdmin && siteUrl.Url.StartsWith("/admin") ||
										   siteUrl.AccessModules.Any(
											   accessModule =>
												   accessModule.UserAccessRights.Any(ar => ar.User.Id == CurrentUser.Id) ||
												   accessModule.RoleAccessRights.Any(ar => ar.Role.Users.Any(u => u.Id == CurrentUser.Id))
												   )
								  )
								  .Select(u => u.Url);

			return urls.ToList();
		}

		public IList<string> GetCurrentUserAllowedFeatures()
		{
			var isSuperAdmin = IsSysAdmin;
			var features = _repository.GetAll<Feature>()
									  .Where(
										  s => isSuperAdmin ||
											   s.AccessModules.Any(
												   m =>
													   m.UserAccessRights.Any(ar => ar.User.Id == CurrentUser.Id) ||
													   m.RoleAccessRights.Any(
														   ar => ar.Role.Users.Any(u => u.Id == CurrentUser.Id))))
									  .Select(u => u.Code)
									  .Distinct()
									  .ToList();

			return features.ToList();
		}

		public Company GetCurrentBizOwner()
		{
			if (CurrentUser != null)
			{
				var user = _repository.Get<User>(CurrentUser.Id);
				if (user != null)
				{
					return user.Company;
				}
			}

			return null;
		}

		public void ChangePassword(User user, string password)
		{
			_externalAuthProvider.BeforeChangePasswordHook(user, password);

			user.PasswordHash = MD5HashCrypto.GetHash(password);
			user.ConfirmationToken = string.Empty;
			user.PasswordVerificationToken = string.Empty;
			user.PasswordVerificationTokenExpirationDate = null;
			user.LastPasswordChangeDate = DateTime.UtcNow;

			_repository.Update(user);
			_repository.SaveChanges();

			if (CurrentUser != null && user.Id == CurrentUser.Id)
				CurrentUser.PasswordHash = user.PasswordHash;
		}

		public void ChangePassword(long userId, string password)
		{
			var user = _repository.Get<User>(userId);
			if (user == null)
				throw new BaseNotFoundException<User>();

			var isSuperAdmin = IsSysAdmin;

			if (!isSuperAdmin && CurrentUser.Id != userId)
			{
				if (!IsAccessAllowed(PermissionAuthorize.Feature(MembershipPermissions.ChangeUserPassword)))
					throw new UnauthorizedAccessException("You are not authorized to access this resource");

				var company = GetCurrentBizOwner();
				if (company.IsMasterCompany)
				{
					if (user.Company != null && user.CompanyId != company.Id &&
						user.Company.MasterCompanyId != company.Id)
						throw new UnauthorizedAccessException(
							"You are not authorized to change password for user that does not belong to your organization/companies");
				}
				else
				{
					if (user.Company == null || user.CompanyId != company.Id)
						throw new UnauthorizedAccessException("You are not authorized to change password for user that does not belong to your organization/companies");
				}
			}

			ChangePassword(user, password);
		}

		public bool IsImpersonated => ImpersonatedFromUser != null;

		public void CancelImpersonation()
		{
			if (ImpersonatedFromUser != null)
			{
				var user = _repository.Get<User>(ImpersonatedFromUser.Id);
				user.ImpersonatedAs = null;

				_repository.Update(user);
				_repository.SaveChanges();
			}
		}

		public void Impersonate(long id)
		{
			if (CurrentUser.Id == id)
				throw new InvalidOperationException("Cannot self-impersonate");

			var impersonateAsUser = _repository.Get<User>(id);

			User user;
			if (IsImpersonated && ImpersonatedFromUser != null)
			{
				user = _repository.Get<User>(ImpersonatedFromUser.Id);
			}
			else
			{
				user = _repository.Get<User>(CurrentUser.Id);
			}

			user.ImpersonatedAs = impersonateAsUser;

			_repository.Update(user);
			_repository.SaveChanges();
		}

		public User GetCurrentUser()
		{
			return CurrentUser;
		}

		/// <summary>
		///     One parameter is required. Either email or externalKey should be provided
		/// </summary>
		/// <param name="email"></param>
		/// <param name="externalKey"></param>
		public void SyncExternalUser(string email, string externalKey)
		{
			_externalAuthProvider.SyncUserHook(email, externalKey);
		}


		public bool TryInviteUser(long userId, out string errMsg)
		{
			errMsg = string.Empty;

			var user = _repository.GetAll<User>().FirstOrDefault(u => u.Id == userId);

			if (user == null)
			{
				errMsg = "User email not found";
				return false;
			}

			var newPassword = Guid.NewGuid().ToString("N").Substring(0, 8);

			//Update sql user password
			user.PasswordHash = MD5HashCrypto.GetHash(newPassword);

			_repository.Update(user);
			_repository.SaveChanges();

			return true;
		}

		private bool IsAccessAllowed(long accessEntityId, User user, AccessKind accessKind = AccessKind.Write)
		{
			if (user == null)
			{
				return false;
			}

			if (user.Id == -1 || (IsImpersonated && ImpersonatedFromUser.Id == -1))
				return true;

			var userRights =
				user.UserAccessRights.Where(r => r.AccessModule.AccessEntities.Any(x => x.Id == accessEntityId))
					.ToList();
			var roleRights =
				user.Roles.SelectMany(
					r => r.RoleAccessRights.Where(ar => ar.AccessModule.AccessEntities.Any(x => x.Id == accessEntityId)))
					.ToList();

			if ((userRights.Any() || roleRights.Any())
				&& userRights.All(ar => ar.AccessRight.AccessKind != AccessKind.Deny)
				&& roleRights.All(ar => ar.AccessRight.AccessKind != AccessKind.Deny))
			{
				return true;
			}
			return false;
		}

		public bool ValidateAuthToken(string token)
		{
			return ValidateUser(token).IsSuccess;
		}
	}
}