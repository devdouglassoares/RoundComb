namespace Membership.Library.Contracts
{
    public interface IAuthorizeService : IDependency
    {
        string CurrentUserName { get; }
        string CurrentUserEmail { get; }

        bool IsPortalUserLoggedIn();

        void Logout();

        bool VerifyChangePasswordParams(string acctId, string pwdResetKey);

        bool VerifyDateOfBirth(string acctId, int year, int month, int day);

        bool VerifyFourDigitSSN(string acctId, string fourDigitSsn);

        bool TryResetUserPassword(string login, out string errMsg);

        bool TryInviteUser(long userId, out string errMsg);

        bool CreatePortalUserAndSendEmail(string ssn, string email, out string errMsg);

        bool ChangePasswordForCurrentUser(string oldPassword, string newPassword, out string errMsg);
        bool ChangePasswordForCurrentUser(long userId, string newPassword, out string errMsg);

        Entities.User LoginAsPortalUser(string ssn);

        //PortalLogin GetPortalLoginByAccId(string acctId);

        void FoxProLogin(string userName);

        //int GetPasswordComplexityParam(string paramName);

        bool VerifyResetPasswordToken(string token);

        void SetUserPassword(string token, string password);

        bool TrySendVerificationCode(string phone, out string err);

        bool CheckVerificationCode(string phone, string code, out string passwordVerificationToken);

        bool IsImpersonated { get; }
    }
}
