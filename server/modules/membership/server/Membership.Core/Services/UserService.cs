using Core.Database;
using Core.Exceptions;
using Core.ObjectMapping;
using Core.SiteSettings;
using Membership.Core.Contracts;
using Membership.Core.Data;
using Membership.Core.Dto;
using Membership.Core.Entities;
using Membership.Core.Entities.Enums;
using Membership.Core.Exceptions;
using Membership.Core.SiteSettings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Membership.Core.Services
{
    public class UserService : BaseService<Entities.User, UserBaseModel>, IUserService
    {
        private const string DeletedIdentifierKey = "_DELETED_";
        private readonly IMappingService _mappingService;
        private readonly IMembership _membership;
        private readonly IPermissionService _permissionService;
        private readonly ICoreRepository _repository;
        private readonly IRoleService _roleService;
        private readonly ISiteSettingService _siteSettingService;
        private MembershipSetting _membershipSetting;

        private MembershipSetting MembershipSetting
        {
            get
            {
                if (_membershipSetting != null) return _membershipSetting;

                _membershipSetting = _siteSettingService.GetSetting<MembershipSetting>();
                return _membershipSetting;
            }
        }

        public UserService(ICoreRepository repository,
                           IMembership membership,
                           IPermissionService permissionService,
                           IRoleService roleService,
                           IMappingService mappingService,
                           ISiteSettingService siteSettingService) : base(mappingService, repository)
        {
            _repository = repository;
            _membership = membership;
            _permissionService = permissionService;
            _roleService = roleService;
            _mappingService = mappingService;
            _siteSettingService = siteSettingService;
        }

        public IQueryable<User> GetUsers()
        {
            var query = _repository.GetAll<User>();

            var company = _membership.GetCurrentBizOwner();

            if (company != null)
            {
                var companyId = company.Id;

                query = company.IsMasterCompany
                            ? query.Where(user => user.CompanyId == null || user.CompanyId == companyId || user.Company.MasterCompanyId == companyId)

                            : query.Where(user => user.CompanyId == companyId);
            }

	        if (!_membership.IsSysAdmin)
	        {
		        query = query.Where(user => user.Roles.All(role => role.Code != MembershipConstant.C_ROLE_CODE_SYSADMIN));
	        }

            return query;
        }

	    public override IQueryable<User> Fetch(Expression<Func<User, bool>> expression)
	    {
		    return GetUsers().Where(expression);
	    }

        public IQueryable<User> GetUsersByRole(long id)
        {
            var role = _repository.Get<Role>(x => x.Id == id);
            return GetUsersByRole(role);
        }

        public IQueryable<User> GetUsersByRoleName(string roleName)
        {
            var role = _repository.Get<Role>(x => x.Name == roleName);

            return GetUsersByRole(role);
        }

        public IQueryable<User> GetUsersByRole(Role role)
        {
            if (role == null)
                throw new BaseNotFoundException<Role>();

            var company = _membership.GetCurrentBizOwner();

            if (company == null)
            {
                return role.Users.AsQueryable();
            }

            var users = _repository.Fetch<User>(user => user.Roles.Any(x => x.Id == role.Id));

            var companyId = company.Id;

            users = company.IsMasterCompany
                        ? users.Where(user => user.CompanyId == null || user.CompanyId == companyId || user.Company.MasterCompanyId == companyId)
                        : users.Where(user => user.CompanyId == companyId);

            return users.AsQueryable();
        }

        public IQueryable<Role> GetRoles()
        {
            return _repository.GetAll<Role>();
        }

        public IList<User> GetUsersByGroup(long id)
        {
            var users = _repository.Fetch<User>(x => x.Groups.Any(group => group.Id == id));

            var company = _membership.GetCurrentBizOwner();

            if (company == null)
            {
                return users.ToList();
            }

            var companyId = company.Id;

            users = company.IsMasterCompany
                        ? users.Where(user => user.CompanyId == null || user.CompanyId == companyId || user.Company.MasterCompanyId == companyId)
                        : users.Where(user => user.CompanyId == companyId);

            return users.ToList();
        }

        public IQueryable<Group> GetGroups()
        {
            var company = _membership.GetCurrentBizOwner();
            return _repository.Fetch<Group>(group => group.Company.Id == company.Id);
        }

        public IQueryable<UserBaseModel> QueryUsers(bool? getDeletedOnly = null)
        {
            var users = _repository.GetAll<User>();
            if (getDeletedOnly != null)
            {
                users = users.Where(x => x.IsDeleted == getDeletedOnly);
            }
            return MappingService.Project<User, UserBaseModel>(users);
        }

        public UserBaseModel Get(long id)
        {
            var user = _repository.Get<User>(id);
            if (user == null || user.IsDeleted)
            {
                throw new BaseNotFoundException<User>();
            }

            var model = _mappingService.Map<UserBaseModel>(user);

            var assignedOtherAccessEntities =
                user.UserAccessRights.SelectMany(x => x.AccessModule.AccessEntities)
                    .Where(x => x.Type == AccessEntityType.Other)
                    .ToList();

            if (assignedOtherAccessEntities.Any())
            {
                model.OtherAccessEntityId = assignedOtherAccessEntities.FirstOrDefault().Id;
            }

            return model;
        }

        public UserPersonalInformation GetUserPersonalInformation(long id)
        {
            var user = _repository.Get<User>(id);
            if (user == null || user.IsDeleted)
            {
                throw new BaseNotFoundException<User>();
            }

            return _mappingService.Map<UserPersonalInformation>(user);
        }

        public IEnumerable<UserPersonalInformation> GetUsersPersonalInformation(long[] ids)
        {
            var users = _repository.Fetch<User>(user => ids.Contains(user.Id));

            return _mappingService.Project<User, UserPersonalInformation>(users);
        }

        public void UpdateUserPersonalInformation(long id, UserPersonalInformation model)
        {
            var user = _repository.Get<User>(id);
            if (user == null || user.IsDeleted)
            {
                throw new BaseNotFoundException<User>();
            }

            var cellPhoneNumber = string.IsNullOrEmpty(model.CellPhoneNumber)
                                      ? string.Empty
                                      : model.CellPhoneNumber.Replace("-", string.Empty).Replace("+", string.Empty);

            if (!string.IsNullOrEmpty(cellPhoneNumber) && cellPhoneNumber != user.CellPhoneNumber)
            {
                if (_repository.Any<User>(u => u.Id != user.Id && (
                                 u.CellPhoneNumber.Replace("-", string.Empty) == (cellPhoneNumber)
                                 ||
                                 cellPhoneNumber == (u.CellPhoneNumber.Replace("-", string.Empty)))))

                    throw new UserPhoneNumberAlreadyInUsedException();
            }

            _mappingService.Map(model, user);
            if (model.CompanyId.HasValue)
            {
                var clientCompany = _repository.Get<Company>(model.CompanyId.Value);
                if (clientCompany != null)
                {
                    user.Company = clientCompany;
                }
            }
            else
            {
                user.Company = null;
                user.CompanyId = null;
            }
            _repository.Update(user);
            _repository.SaveChanges();
        }

        public bool Update(UserBaseModel model)
        {
            var user = _repository.Get<User>(model.Id);
            if (user == null || user.IsDeleted)
            {
                throw new BaseNotFoundException<User>();
            }

            var cellPhoneNumber = model.CellPhoneNumber.Replace("-", string.Empty).Replace("+", string.Empty);

            if (!string.IsNullOrEmpty(cellPhoneNumber) && cellPhoneNumber != user.CellPhoneNumber)
            {
                if (_repository.Any<User>(u => u.Id != user.Id && (
                                 u.CellPhoneNumber.Replace("-", string.Empty) == (cellPhoneNumber)
                                 ||
                                 cellPhoneNumber == (u.CellPhoneNumber.Replace("-", string.Empty)))))

                    throw new UserPhoneNumberAlreadyInUsedException();
            }

            _mappingService.Map(model, user);

            if (model.BizOwnerId.HasValue)
            {
                var company = _repository.Get<Company>(model.BizOwnerId.Value);
                user.Company = company;
            }
            else
            {
                user.Company = null;
            }

            if (!string.IsNullOrEmpty(model.Password))
            {
                _membership.ChangePassword(user, model.Password);
            }

            var otherUserAccessRights =
                user.UserAccessRights.Where(
                    x => x.AccessModule.AccessEntities.Any(ae => ae.Type == AccessEntityType.Other)).ToList();

            if (otherUserAccessRights.Any())
            {
                foreach (var otherUserAccessRight in otherUserAccessRights)
                {
                    _repository.Delete(otherUserAccessRight);
                }

                _repository.SaveChanges();
            }

            _repository.Update(user);
            _repository.SaveChanges();

            if (model.OtherAccessEntityId.HasValue)
            {
                _permissionService.AssignToUser(user.Id, model.OtherAccessEntityId.Value);
            }

            AssignDefaultRoleToUser(user);

            return true;
        }

        public UserBaseModel Register(UserBaseModel model)
        {
            var result = _membership.RegisterUser(model.FirstName,
                                                  model.LastName,
                                                  model.Comment,
                                                  model.Email,
                                                  model.Password,
                                                  model.CellPhoneNumber,
                                                  model.HomePhoneNumber,
                                                  model.Address,
                                                  companyId: model.CompanyId,
                                                  isVirtualUser: model.IsVirtual);

            if (!result.IsSuccess)
            {
                throw new Exception("Error while creating new user");
            }

            var user = _repository.Get<User>(result.UserId);

            //if (model.ClientCompanyId.HasValue)
            //{
            //    user.ClientCompany = _repository.Get<Company>(model.ClientCompanyId.Value);
            //    _repository.Update(user);
            //    _repository.SaveChanges();
            //}


            if (model.Roles != null && model.Roles.Any())
            {
                foreach (var roleCode in model.Roles)
                {
                    var role = GetRoles().FirstOrDefault(x => x.Code == roleCode || x.Name == roleCode);
                    if (role != null)
                    {
                        _roleService.AssignToUser(user.Id, role.Id);
                    }
                }
            }

            AssignDefaultRoleToUser(user);

            if (model.OtherAccessEntityId.HasValue)
            {
                _permissionService.AssignToUser(user.Id, model.OtherAccessEntityId.Value);
            }

            return _mappingService.Map<UserBaseModel>(user);
        }

        private void AssignDefaultRoleToUser(User user)
        {
            if (MembershipSetting.DefaultRoleToAssignToNewRegistration > 0)
            {
                var role = _repository.Get<Role>(MembershipSetting.DefaultRoleToAssignToNewRegistration);

                if (role != null)
                    user.Roles.Add(role);
            }
        }

        public bool Delete(long id)
        {
            var result = false;
            const string C_METHOD = "Delete";

            try
            {
                var user = _repository.Get<User>(id);

                if (user != null && !user.IsDeleted)
                {
                    user.FollowedUsers.Clear();
                    user.FollowedByUsers.Clear();

                    //_repository.Remove(user);

                    user.Email = DateTime.Now.ToString("u") + DeletedIdentifierKey + user.Email;
                    user.CellPhoneNumber = DateTime.Now.ToString("u") + DeletedIdentifierKey + user.CellPhoneNumber;
                    user.IsDeleted = true;

                    _repository.Update(user);

                    _repository.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("[" + C_METHOD + "] - " + ex.Message);
            }

            // [delete][SaveChanges] - failed on connecting database. liklye port issue ask admin to open port 81. 
            return result;
        }

        public void ActivateUser(long id)
        {
            var user = _repository.Get<User>(id);
            if (user == null || user.IsDeleted)
                throw new BaseNotFoundException<User>();

            user.IsActive = true;
            user.IsLockedOut = false;
            _repository.Update(user);
            _repository.SaveChanges();
        }

        public void DeactivateUser(long id)
        {
            var user = _repository.Get<User>(id);
            if (user == null || user.IsDeleted)
                throw new BaseNotFoundException<User>();

            user.IsActive = false;
            user.IsLockedOut = true;
            _repository.Update(user);
            _repository.SaveChanges();
        }

        public bool IsEmailExists(string email)
        {
            var company = _membership.GetCurrentBizOwner();

            User user;
            if (company != null)
                user = _repository.Get<User>(u => u.Email == email && u.Company.Id == company.Id);
            else
                user = _repository.Get<User>(u => u.Email == email && u.Company == null);

            return user != null;
        }

        public long[] GetUserIdsInLocations(long[] locationIds)
        {
            var userProfiles =
                _repository.Fetch<UserProfile>(
                                               profile =>
                                               profile.LocationId != null &&
                                               locationIds.Contains(profile.LocationId.Value))
                           .Select(profile => profile.UserId);

            return userProfiles.ToArray();
        }

        /*public UserBaseModel GetUserByExternalKey(string key)
        {
            var user = this.repository.GetAll<User>().FirstOrDefault(x => x.ExternalKey == key);
            return _mappingService.Map<UserBaseModel>(user);
        }

        public void CreateUserByExternalKey(string key, string firstName, string lastName)
        {
            var company = this.membership.GetCurrentBizOwner();

            if (company == null)
            {
                throw new Exception("Biz owner is null");
            }

            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                ExternalKey = key,
                IsActive = true,
                IsApproved = true,
                CreateDate = DateTime.Now,
                Company = this.membership.GetCurrentBizOwner(),
                Roles = new List<Role> { this.repository.GetAll<Role>().FirstOrDefault(r => r.Code == MembershipConstant.C_ROLE_CODE_COMPANYCLIENT) }
            };

            this.repository.Insert(user);
            this.repository.SaveChanges();
        }

        private void InsertUserByExternalKey(string key, UserBaseModel model)
        {
            var company = this.membership.GetCurrentBizOwner();

            if (company == null)
            {
                throw new Exception("Biz owner is null");
            }

            var user = new User
            {
                ExternalKey = key,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                IsActive = true,
                IsApproved = true,
                CreateDate = DateTime.Now,
                Company = company,
                Roles = new List<Role> { this.repository.GetAll<Role>().FirstOrDefault(r => r.Code == MembershipConstant.C_ROLE_CODE_COMPANYCLIENT) }
            };

            this.repository.Insert(user);
            this.repository.SaveChanges();
        }*/

        public void BulkCreateUserByExternalKey(long companyId, List<ImportUserDto> importedUsers)
        {
            var company = _repository.Get<Company>(companyId);

            if (company == null)
            {
                throw new Exception("Biz owner is null");
            }

            _repository.DisableAutoDetectChanges();

            var thisCompanyUsers = _repository.GetAll<User>().Where(x => x.Company.Id == company.Id).ToList();

            try
            {
                foreach (var importedUser in importedUsers)
                {
                    var user = thisCompanyUsers.FirstOrDefault(x => x.ExternalKey == importedUser.ExternalKey);

                    if (user != null)
                    {
                        if (user.FirstName != importedUser.FirstName
                            || user.LastName != importedUser.LastName
                            || user.Address != importedUser.Address)
                        {
                            user.FirstName = importedUser.FirstName;
                            user.LastName = importedUser.LastName;
                            user.Address = importedUser.Address;
                            //user.CellPhoneNumber = importedUser.Phone;

                            if (user.Contacts == null)
                                user.Contacts = new List<Contact>();

                            user.Contacts.Add(new Contact
                            {
                                Type = MembershipConstant.ContactTypePhone,
                                Value = importedUser.Phone
                            });

                            _repository.Update(user);
                            //this.repository.SaveChanges();
                        }

                    }
                    else
                    {
                        _repository.Insert(new User
                        {
                            ExternalKey = importedUser.ExternalKey,
                            FirstName = importedUser.FirstName,
                            LastName = importedUser.LastName,
                            Address = importedUser.Address,
                            //CellPhoneNumber = importedUser.Phone,
                            IsActive = true,
                            IsApproved = true,
                            Company = company,
                            Roles =
                                                   new List<Role>
                                                   {
                                                       _repository.GetAll<Role>()
                                                                  .FirstOrDefault(r => r.Code == importedUser.Role)
                                                   },
                            Contacts = new List<Contact>
                                       {
                                           new Contact
                                           {
                                               Type = MembershipConstant.ContactTypePhone,
                                               Value = importedUser.Phone
                                           }
                                       }
                        });
                    }

                    _repository.SaveChanges();
                }
            }
            finally
            {
                _repository.EnableAutoDetectChanges();
            }
        }

        public void MergeUsers(long userId, long mergeWithId)
        {
            var user = _repository.GetAll<User>().FirstOrDefault(x => x.Id == userId);

            if (user != null)
            {
                user.ExternalKey = string.Format("MERGED#{0}", mergeWithId);

                _repository.Update(user);
                _repository.SaveChanges();
            }
        }

        public User GetUser(long userId)
        {
            if (userId == 0)
            {
                return null;
            }

            var user = _repository.Get<User>(userId);

            if (user == null)
            {
                return null;
            }

            if (!string.IsNullOrEmpty(user.ExternalKey))
            {
                if (user.ExternalKey.StartsWith("MERGED#"))
                {
                    int mergedToUserId;
                    if (int.TryParse(user.ExternalKey.Substring(7), out mergedToUserId))
                    {
                        var mergedToUser = _repository.Get<User>(mergedToUserId);
                        if (mergedToUser != null)
                        {
                            user = mergedToUser;
                        }
                    }
                }
            }

            return user;
        }

        public User GetUserByExternalKey(string externalKey)
        {
            var user = _repository.Get<User>(u => u.ExternalKey == externalKey);

            return user;
        }

        public User GetUserByEmail(string email)
        {
            var user = _repository.Get<User>(u => u.Email == email);

            return user;
        }

        public void RegisterUserDevice(string deviceId)
        {
            var user = _repository.Get<User>(_membership.UserId);


            var devices = (string.IsNullOrEmpty(user.Devices) ? new string[0] : user.Devices.Split(';')).ToList();
            if (!devices.Contains(deviceId))
            {
                devices.Add(deviceId);
                user.Devices = string.Join(";", devices);
                _repository.Update(user);
                _repository.SaveChanges();
            }
        }

        public void UnregisterUserDevice(string deviceId)
        {
            var user = _repository.Get<User>(_membership.UserId);


            var devices = (string.IsNullOrEmpty(user.Devices) ? new string[0] : user.Devices.Split(';')).ToList();
            if (devices.Contains(deviceId))
            {
                devices.Remove(deviceId);
                user.Devices = string.Join(";", devices);
                _repository.Update(user);
                _repository.SaveChanges();
            }
        }

        public void UpdateExternalToken(long userId, UserExternalLogin externalLogin)
        {
            var user = this._repository.Get<User>(userId);

            var existingLogin = user.ExternalLogins.FirstOrDefault(l => l.ExternalProviderName == externalLogin.ExternalProviderName);
            if (existingLogin == null)
            {
                user.ExternalLogins.Add(externalLogin);
            }
            else
            {
                existingLogin.AccessKey = externalLogin.AccessKey;
                existingLogin.SecretKey = externalLogin.SecretKey;
            }

            this._repository.Update(user);
            this._repository.SaveChanges();
        }

        public UserPersonalInformation GetUserByPhoneNumber(string phoneNumber)
        {
            var cellPhoneNumber = phoneNumber.Replace("-", string.Empty).Replace("+", string.Empty);

            var user =
                _repository.Get<User>(
                    u =>
                    !u.IsDeleted &&
                    u.CellPhoneNumber != null && (u.CellPhoneNumber.Replace("-", string.Empty).Contains(cellPhoneNumber)
                                                  ||
                                                  cellPhoneNumber.Contains(u.CellPhoneNumber.Replace("-", string.Empty))));

            return MappingService.Map<UserPersonalInformation>(user);
        }
        public bool IsEmptyAddresses()
        {
            return _repository.GetAll<User>().Any(x => string.IsNullOrEmpty(x.Address));
        }

        public void UndoDeletion(long id)
        {
            var user = _repository.Get<User>(id);
            if (user == null || !user.IsDeleted)
                throw new BaseNotFoundException<User>();
            user.IsDeleted = false;
            user.Email = user.Email.Split(new [] { "DELETED_" }, StringSplitOptions.None)[1];
            user.CellPhoneNumber = user.CellPhoneNumber.Split(new[] {"DELETED_"}, StringSplitOptions.None)[1];
            _repository.Update(user);
            _repository.SaveChanges();
        }
    }
}