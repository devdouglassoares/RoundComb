using Membership.Core.Entities;

namespace Membership.Core.ExternalProviders
{
    public interface IExternalAuthProvider
    {
        void BeforeLoginHook(string email, string password, MembershipResult result);
        void AfterLoginHook(string email, string password, MembershipResult result);

        void BeforeRegisterHook(string email, string password, MembershipResult result);
        void AfterRegisterHook(string email, string password, MembershipResult result);
        void BeforeChangePasswordHook(User user, string password);

        void SyncUserHook(string email, string externalKey);
    }
}
