using Core;
using Core.Database;
using Membership.Core.Dto;
using Membership.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Membership.Core.Contracts
{
    public interface IUserService : IBaseService<Entities.User, UserBaseModel>, IDependency
    {
        IQueryable<Entities.User> GetUsers();

        IQueryable<Entities.User> GetUsersByRole(long id);

        IQueryable<Entities.User> GetUsersByRoleName(string roleName);

        IQueryable<Role> GetRoles();

        IList<Entities.User> GetUsersByGroup(long id);

        IQueryable<Group> GetGroups();
        IQueryable<UserBaseModel> QueryUsers(bool? getDeletedOnly = null);

        UserBaseModel Get(long id);

        UserPersonalInformation GetUserPersonalInformation(long id);
        IEnumerable<UserPersonalInformation> GetUsersPersonalInformation(long[] ids);


        void UpdateUserPersonalInformation(long id, UserPersonalInformation model);

        bool Update(UserBaseModel model);

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="model"></param>
        /// <exception cref="UserEmailAlreadyInUsedException">Throw if user already exist</exception>
        /// <returns></returns>
        UserBaseModel Register(UserBaseModel model);

        bool Delete(long id);

        void ActivateUser(long id);

        void DeactivateUser(long id);

        bool IsEmailExists(string email);

        long[] GetUserIdsInLocations(long[] locationIds);

        //UserBaseModel GetUserByExternalKey(string key);
        //void CreateUserByExternalKey(string key, string firstName, string lastName);
        void BulkCreateUserByExternalKey(long companyId, List<ImportUserDto> importedUsers);
        void MergeUsers(long userId, long mergeWithId);
        Entities.User GetUser(long userId);
        Entities.User GetUserByExternalKey(string externalKey);
        Entities.User GetUserByEmail(string email);
        void RegisterUserDevice(string deviceId);
        void UnregisterUserDevice(string deviceId);
        void UpdateExternalToken(long userId, UserExternalLogin externalLogin);
        UserPersonalInformation GetUserByPhoneNumber(string phoneNumber);
        bool IsEmptyAddresses();
        void UndoDeletion(long id);
    }
}
