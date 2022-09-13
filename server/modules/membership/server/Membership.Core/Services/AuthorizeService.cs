using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Membership.Library.Contracts;
using Membership.Library.Data;
using Membership.Library.Entities;
using Membership.Library.Implementation;
using Notifications.Client;
using Notifications.Client.Models;

namespace Membership.Library.Services
{
    public class AuthorizeService : IAuthorizeService
    {
        private readonly IRepository repository;

        //private readonly IFoxproRepository foxproRepository;

        private readonly IMembership membership;
//        private readonly INotificationService notificationService;

//        private readonly IConfigurationService configurationService;

        public AuthorizeService(IRepository repository/*, IFoxproRepository foxproRepository*/, IMembership membership
//            , INotificationService notificationService
            )
        {
            this.repository = repository;
            //this.foxproRepository = foxproRepository;
            this.membership = membership;
//            this.notificationService = notificationService;
//            this.configurationService = new ConfigurationService();
        }

        private static MD5 md5 = null;
        private static string MD5Str(string hashStr)
        {
            if (md5 == null) md5 = MD5.Create();
            return BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(hashStr))).ToUpper().Replace("-", "").Trim();
        }

        public bool VerifyChangePasswordParams(string acctId, string pwdResetKey)
        {
            return false;
            /*var login = this.foxproRepository.FindUserByAccountId(acctId);
            if (login == null) return false;

            login.GetResetKeyAndExpireIfNecessary(
                TimeSpan.FromSeconds(this.configurationService.ResetKeyExpirationInSeconds));

            return pwdResetKey.Equals(login.ResetKey);*/
        }



        public bool VerifyDateOfBirth(string acctId, int year, int month, int day)
        {
            return false;
            /*var login = this.foxproRepository.FindUserByAccountId(acctId);

            return login.PatientIfPossible.DOB.Year == year
                   && login.PatientIfPossible.DOB.Month == month
                   && login.PatientIfPossible.DOB.Day == day;*/
        }

        public bool VerifyFourDigitSSN(string acctId, string fourDigitSsn)
        {
            return false;
        }

        public bool TryResetUserPassword(string login, out string errMsg)
        {
            errMsg = string.Empty;

            var domain = HttpContext.Current.Request.Url.Host.Split('.').First();
            var company = this.repository.Query<Company>().FirstOrDefault(c => c.Domain == domain);
            User user;
            if (company == null)
            {
                user = repository.Query<User>().FirstOrDefault(u => u.Email == login);
            }
            else
            {
                if (company.Owner.Email.Equals(login, StringComparison.InvariantCultureIgnoreCase))
                {
                    user = company.Owner;
                }
                else
                {
                    user = this.repository.Get<User>(u => u.Email == login && u.Company.Id == company.Id);
                }

                // if not found - look for user without company
                if (user == null)
                {
                    var owners = this.repository.Query<Company>().Select(c => c.Owner.Id).ToList();
                    user = repository.Query<User>().FirstOrDefault(u => u.Email == login && u.Company == null && !owners.Contains(u.Id));

                }
            }

            //var user = this.repository.Query<User>().FirstOrDefault(u => u.Email == login);

            if (user == null)
            {
                errMsg = "User email not found";
                return false;
            }

            user.PasswordVerificationToken = Guid.NewGuid().ToString();
            user.PasswordVerificationTokenExpirationDate = DateTime.UtcNow.AddDays(7);

            var request = HttpContext.Current.Request;
            string baseUrl = request.Url.Scheme + "://" + request.Url.Authority + request.ApplicationPath.TrimEnd('/') + "/Account/ChangePassword/";
//            this.notificationService.SendEmailConfirmation("Share430", baseUrl, user.FirstName + " " + user.LastName,
//                user.PasswordVerificationToken, login);


            this.repository.Update(user);
            this.repository.SaveChanges();
            return true;
        }

        public bool TryInviteUser(long userId, out string errMsg)
        {
            errMsg = string.Empty;

            var user = this.repository.Query<User>().FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                errMsg = "User email not found";
                return false;
            }

            var newPassword = System.Guid.NewGuid().ToString("N").Substring(0, 8);

            //Update sql user password
            user.PasswordHash = MD5HashCrypto.GetHash(newPassword);

            this.repository.Update(user);
            this.repository.SaveChanges();

            var notificationsClient = new NotificationsClient("https://notifications.remindsimple.com/notification");

            var request = HttpContext.Current.Request;
            string baseUrl = request.Url.Scheme + "://" + request.Url.Authority;

            var result = notificationsClient.SendNotification(new NotificationModel
            {
                NotificationType = NotificationTypes.Email,
                Subject = "Portal User Invitation",
                Message = string.Format("Hi, {0} {1}. You were invited to {2}. Your temporary password is {3}", user.FirstName, user.LastName, baseUrl, newPassword),
                Targets = new List<string>{ user.Email}
            });

            //var request = HttpContext.Current.Request;
            //string baseUrl = request.Url.Scheme + "://" + request.Url.Authority + request.ApplicationPath.TrimEnd('/') + "/";
//            this.notificationService.SendEmailInvitation("Share430", baseUrl, user.Company.Owner.FirstName + " " + user.Company.Owner.LastName, user.FirstName + " " + user.LastName,
//                newPassword, user.Email);

            return true;
        }

        public bool CreatePortalUserAndSendEmail(string ssn, string email, out string errMsg)
        {
            errMsg = string.Empty;
            /*var patient = this.foxproRepository.FindPatientBySsn(ssn);
            if (patient == null)
            {
                errMsg = "Patient with this ssn is not exist";
                return false;
            }

            if (!this.foxproRepository.CreatePortalUser(ssn, email, patient.FirstName, patient.LastName))
            {
                errMsg = "Error creating portal user";
                return false;
            }

            var foxProUser = this.foxproRepository.FindUserByLogin(email);
            var newAcctEmail = this.foxproRepository.GetNewAccountEmailLetter();
            if (newAcctEmail == null)
            {
                errMsg = "No new account email defined";
                return false;
            }

            if (foxProUser == null)
            {
                errMsg = "Login not exist";
                return false;
            }

            foxProUser.EmailAddress = email;
            int newAcctLinkExpiration;
            if (int.TryParse(WebConfigurationManager.AppSettings["NEW_ACCT_LINK_EXP_DAYS"], out newAcctLinkExpiration))
            {
                this.foxproRepository.ResetPasswordAndSendEmail(foxProUser, newAcctEmail, DateTime.Now.AddDays(newAcctLinkExpiration));
            }
            else
            {
                this.foxproRepository.ResetPasswordAndSendEmail(foxProUser, newAcctEmail, DateTime.Now.AddDays(3));
            }
            return true;*/
            return false;
        }

        public bool ChangePasswordForCurrentUser(string oldPassword, string newPassword, out string errMsg)
        {
            errMsg = string.Empty;
            /*if (membership.IsCurrentUserInRole("ProgramManager"))
            {
                var adminUser = this.repository.Get<User>(x => x.Email == membership.Email);
                if (MD5HashCrypto.GetHash(oldPassword) != adminUser.PasswordHash)
                {
                    errMsg = "Incorrect password";
                    return false;
                }

                adminUser.PasswordHash = MD5HashCrypto.GetHash(newPassword);
                this.repository.SaveChanges();
                return true;
            }

            if (PersistentState.PortalUser == null)
            {
                errMsg = "Current user is not logged in";
                return false;
            }

            var foxProUser = PersistentState.PortalUser;

            var hashedPassword = MD5Str(MD5Str(foxProUser.ID.ToUpper().Trim() + oldPassword) + this.foxproRepository.GetChallenge());
            bool foxProPasswordIsValid = foxProUser.ValidatePassword(hashedPassword, this.foxproRepository.GetChallenge());

            if (!foxProPasswordIsValid)
            {
                errMsg = "Incorrect password";
                return false;
            }

            foxProUser.Password = newPassword;
            var user = this.repository.Get<User>(x => x.Email == foxProUser.Account);
            if (user == null)
            {
                user = new User
                {
                    Ssn = foxProUser.PatientIfPossible != null ? foxProUser.PatientIfPossible.SSN : string.Empty,
                    Email = foxProUser.Account,
                    IsActive = true,
                    IsApproved = true,
                    CreateDate = DateTime.Now,
                    PasswordHash = MD5HashCrypto.GetHash(newPassword),
                    Roles = new Collection<Role>()
                };

                this.repository.Insert(user);
                var patientRole = this.repository.Get<Role>(x => x.Name == "Patient");
                user.Roles.Add(patientRole);
                this.repository.SaveChanges();
            }
            else
            {
                //Update sql user password
                user.PasswordHash = MD5HashCrypto.GetHash(newPassword);
                this.repository.SaveChanges();
            }

            return true;*/
            return false;
        }

        public bool ChangePasswordForCurrentUser(long userId, string newPassword, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                var user = this.repository.Get<User>(x => x.Id == userId);
                user.PasswordHash = MD5HashCrypto.GetHash(newPassword);
                this.repository.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.ToString();
            }

            return false;
        }

        public User LoginAsPortalUser(string ssn)
        {
            /*var login = this.foxproRepository.FindUserBySsn(ssn);
            this.foxproRepository.LoginUserToPersistentState(login);

            var user = this.repository.Get<User>(x => x.Email == login.Account);
	        return user;*/
            return null;
        }

        public string CurrentUserName
        {
            get
            {
                /*if (PersistentState.PortalUser != null)
                {
                    return PersistentState.PortalUser.Name;
                }
                else
                {
                    return this.membership.Name;
                }*/
                return this.membership.Name;
            }
        }

        public string CurrentUserEmail
        {
            get
            {
                return this.membership.Email;
            }
        }

        public bool IsPortalUserLoggedIn()
        {
            return false;
            //return PersistentState.PortalUser != null;
        }

        public void Logout()
        {
            //this.foxproRepository.LogoutCurrentUserFromPersistentState();
        }

        /*public PortalLogin GetPortalLoginByAccId(string acctId)
	    {
            return this.foxproRepository.FindUserByAccountId(acctId);
	    }*/

        public void FoxProLogin(string userName)
        {
            //var foxProUser = this.foxproRepository.FindUserByLogin(userName);
            //if (foxProUser != null)
            //{
            //this.foxproRepository.LoginUserToPersistentState(foxProUser);
            //}
        }

        public bool VerifyResetPasswordToken(string token)
        {
            return this.repository.Query<User>().Any(x =>
                x.PasswordVerificationToken == token
                && x.PasswordVerificationTokenExpirationDate >= DateTime.UtcNow);
        }

        public void SetUserPassword(string token, string password)
        {
            var user = this.repository.Query<User>()
                .Single(x => x.PasswordVerificationToken == token);
            membership.ChangePassword(user, password);
        }

        public bool TrySendVerificationCode(string phone, out string err)
        {
            throw new NotImplementedException();
        }

//        public bool TrySendVerificationCode(string phone, out string err)
//        {
//            err = string.Empty;
//            var phoneNumber = phone.Replace("-", string.Empty).Replace(" ", string.Empty);
//            Expression<Func<User, bool>> phoneNumberQuery = u => (u.PhoneNumber ?? "").Replace("-", string.Empty).Replace(" ", string.Empty) == phoneNumber;
//            //Func<User, bool> phoneNumberQuery = u => u.PhoneNumber.Replace("-", string.Empty).Replace(" ", string.Empty) == phoneNumber;
//
//            var domain = HttpContext.Current.Request.Url.Host.Split('.').First();
//            var company = this.repository.Query<Company>().FirstOrDefault(c => c.Domain == domain);
//            User user;
//            if (company == null)
//            {
//                user = repository.Query<User>().FirstOrDefault(phoneNumberQuery);
//            }
//            else
//            {
//                if (phoneNumberQuery.Compile().Invoke(company.Owner))
//                {
//                    user = company.Owner;
//                }
//                else
//                {
//                    user = this.repository.Get<User>(u => u.PhoneNumber.Replace("-", string.Empty).Replace(" ", string.Empty) == phoneNumber && u.Company.Id == company.Id);
//                }
//
//                // if not found - look for user without company
//                if (user == null)
//                {
//                    var owners = this.repository.Query<Company>().Select(c => c.Owner.Id).ToList();
//                    user = repository.Query<User>().FirstOrDefault(u => u.PhoneNumber.Replace("-", string.Empty).Replace(" ", string.Empty) == phoneNumber && u.Company == null && !owners.Contains(u.Id));
//
//                }
//            }
//
//
//
//            //var user = this.repository.Get<User>(x => x.PhoneNumber == phone);
//            if (user == null)
//            {
//                err = "Can't find user with this phone";
//                return false;
//            }
//
//            user.ConfirmationToken = CodeGenerator.GetNumberCode(7);
//            this.repository.Update(user);
//            this.repository.SaveChanges();
//
//            //send verification code
//            string accountSid = ConfigurationManager.AppSettings["TwilioAccSid"];
//            string authToken = ConfigurationManager.AppSettings["TwilioAuthToken"];
//            string from = ConfigurationManager.AppSettings["TwilioFrom"];
//
//            var twilio = new TwilioRestClient(accountSid, authToken);
//            var message = twilio.SendMessage(from, user.PhoneNumber, "Verification Code: " + user.ConfirmationToken, "");
//
//            if (message.RestException != null)
//            {
//                err = "Error sending sms, check phone number format";
//                return false;
//            }
//
//            return true;
//        }

        public bool CheckVerificationCode(string phone, string code, out string passwordVerificationToken)
        {
            passwordVerificationToken = string.Empty;
            var phoneNumber = phone.Replace("-", string.Empty).Replace(" ", string.Empty);
            var user = this.repository.Get<User>(x => x.CellPhoneNumber.Replace("-", string.Empty).Replace(" ", string.Empty) == phoneNumber && x.ConfirmationToken == code);
            if (user == null) return false;

            passwordVerificationToken = Guid.NewGuid().ToString();
            user.PasswordVerificationToken = passwordVerificationToken;
            user.PasswordVerificationTokenExpirationDate = DateTime.UtcNow.AddDays(7);

            this.repository.Update(user);
            this.repository.SaveChanges();

            return true;
        }

        public bool IsImpersonated
        {
            get { return this.membership.IsImpersonated; }
        }

        /*public int GetPasswordComplexityParam(string paramName)
        {
            return this.foxproRepository.GetIntegerPref(paramName);
        }*/
    }
}
