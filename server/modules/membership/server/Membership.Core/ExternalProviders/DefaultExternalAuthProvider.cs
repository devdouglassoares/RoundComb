using Membership.Core.Entities;

namespace Membership.Core.ExternalProviders
{
    public class DefaultExternalAuthProvider : IExternalAuthProvider
    {
        public virtual void BeforeLoginHook(string email, string password, MembershipResult result)
        {
            // Do nothing by default
        }

        public virtual void AfterLoginHook(string email, string password, MembershipResult result)
        {
            // Do nothing by default
        }

        public virtual void BeforeRegisterHook(string email, string password, MembershipResult result)
        {
            // Do nothing by default
        }

        public virtual void AfterRegisterHook(string email, string password, MembershipResult result)
        {
            // Do nothing by default
        }

        public virtual void BeforeChangePasswordHook(User user, string password)
        {
            // Do nothing by default
        }

        public virtual void SyncUserHook(string email, string externalKey)
        {
            // Do nothing by default
        }
    }
}