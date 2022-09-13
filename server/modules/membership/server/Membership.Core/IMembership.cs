using Core;
using Membership.Core.Entities;
using Membership.Core.Entities.Enums;
using Membership.Core.Models;
using System.Collections.Generic;

namespace Membership.Core
{
    public interface IMembership : IDependency
    {
        MembershipResult ValidateUser(string login, string password, bool ignorePassword = false);

        MembershipResult ValidateUser(string token);

        string CreateToken();

        string CreateTokenForUser(User user);

        MembershipResult RegisterUser(string email);

        MembershipResult RegisterUser(string firstName, string lastName, string comment, string email, string password, string cellPhoneNumber, string homePhoneNumber, string address, string role = null, string externalKey = null, long? companyId = null, bool isVirtualUser = false);

        string Email { get; }

        string Name { get; }

        bool IsSysAdmin { get; }

        long UserId { get; }

        string UserExternalId { get; }

        IList<Role> GetCurrentUserRoles();

        bool IsCurrentUserInRole(string roleName);

        //void SetCurrentUser(int id);
        //User GetCurrentUser();

        #region User permissions checking

        Dictionary<AccessEntity, AccessRight> GetCurrentUserCalculatedPermissions();

        bool IsAccessAllowed(AccessEntity entity, AccessKind accessKind = AccessKind.Write);

        bool IsAccessAllowed(PermissionAuthorize model, AccessKind accessKind = AccessKind.Write);
        bool IsAccessAllowed(PermissionAuthorize model, long userId, AccessKind accessKind = AccessKind.Write);

        bool IsAccessAllowed(PortalFeatures feature, AccessKind accessKind = AccessKind.Write);

        IList<string> GetCurrentUserAllowedPages();

        IList<string> GetCurrentUserAllowedFeatures();

        #endregion User permissions checking

        Company GetCurrentBizOwner();

        void ChangePassword(User user, string password);

        void ChangePassword(long userId, string password);

        bool IsImpersonated { get; }

        void CancelImpersonation();

        void Impersonate(long id);
		
        User GetCurrentUser();

        void SyncExternalUser(string email, string externalKey);

        bool TryInviteUser(long userId, out string errMsg);
    }
}
