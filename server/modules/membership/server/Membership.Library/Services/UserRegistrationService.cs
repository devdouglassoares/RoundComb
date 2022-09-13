using Core.Events;
using Core.Exceptions;
using Core.Extensions;
using Core.SiteSettings;
using Core.Templating.Services;
using Membership.Core;
using Membership.Core.Contracts.Common;
using Membership.Core.Dto;
using Membership.Core.Entities;
using Membership.Core.Events;
using Membership.Core.Exceptions;
using Membership.Core.Implementation;
using Membership.Core.Models;
using Membership.Core.SiteSettings.Models;
using Membership.Library.Contracts;
using Membership.Library.Data;
using Membership.Library.Dto;
using Membership.Library.Templates;
//using NotifyService.RestClient.Services;
//using notifyService.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Membership.Library.Services
{
	public class UserRegistrationService : IUserRegistrationService
	{
		private const string AdminContactEmailNotConfiguredMessage = "Administrator contact email has not been configured";
		private const string DateShortFormat = "MM/dd/yyyy";

		private readonly IEventPublisher _eventPublisher;
		private readonly IMembership _membership;
		//private readonly INotificationService _notificationService;
		private readonly IRepository _repository;
		private readonly ISiteSettingService _siteSettingService;
		private readonly ITemplateService _templateService;

        private GeneralSiteSetting _generalSiteSetting;
		private MembershipSetting _membershipSetting;

		public UserRegistrationService(IRepository repository,
									   //INotificationService notificationService,
									   ISiteSettingService siteSettingService,
									   ITemplateService templateService,
									   IMembership membership,
									   IEventPublisher eventPublisher)
		{
			_repository = repository;
			//_notificationService = notificationService;
			_siteSettingService = siteSettingService;
			_templateService = templateService;
			_membership = membership;
			_eventPublisher = eventPublisher;
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

		private GeneralSiteSetting GeneralSiteSetting
		{
			get
			{
				if (_generalSiteSetting != null) return _generalSiteSetting;

				_generalSiteSetting = _siteSettingService.GetSetting<GeneralSiteSetting>();
				return _generalSiteSetting;
			}
		}

		public UserLogInTokenModel Register(UserRegistrationModel model, Uri requestUri)
		{
			var isEmailUsed = _repository.Any<User>(x => x.Email == model.Email);

			if (isEmailUsed)
				throw new UserEmailAlreadyInUsedException();

            var user = new User
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                IsActive = true,
                IsApproved = true,
                PasswordHash = string.IsNullOrEmpty(model.Password)
                                              ? string.Empty
                                              : MD5HashCrypto.GetHash(model.Password),
                CellPhoneNumber = model.CellPhoneNumber,
                HomePhoneNumber = model.HomePhoneNumber,
                MiddleName = model.ExternalProvider,
            };

			if (MembershipSetting.RegistrationApprovalMustBeDoneByAdmin)
				user.IsApproved = false;

			if (MembershipSetting.UserMustActivateRegistration)
				user.IsActive = false;

			if (model.Roles == null || !model.Roles.Any())
			{
				if (MembershipSetting.DefaultRoleToAssignToNewRegistration > 0)
				{
					var role = _repository.Get<Role>(MembershipSetting.DefaultRoleToAssignToNewRegistration);

					if (role != null)
						user.Roles.Add(role);
				}
			}
			else
			{
				foreach (var modelRole in model.Roles)
				{
					var role = _repository.Get<Role>(modelRole);

					if (role != null)
						user.Roles.Add(role);
				}
			}

			user.RegistrationDate = DateTimeOffset.Now;
			_repository.Insert(user);
			_repository.SaveChanges();

			_eventPublisher.Publish(new OnUserRegisteredEvent
			{
				User = user,
				RegistrationModel = model
			});

            /*
			// notify admin for new registration
			if (MembershipSetting.NotifyAdministratorForNewRegistrations)
				InformAdminForUserRegistered(user);

			// request user for activation
			if (MembershipSetting.UserMustActivateRegistration)
				SendActivationEmail(user, requestUri);

			// inform admin for approving the registration
			else if (MembershipSetting.RegistrationApprovalMustBeDoneByAdmin)
				InformAdminForApproval(user);

			if ((MembershipSetting.UserMustActivateRegistration ||
				 MembershipSetting.RegistrationApprovalMustBeDoneByAdmin) &&
				!MembershipSetting.AllowNonActivatedUserLogIn)
				return null;
            */

            // request user for activation
            if (MembershipSetting.UserMustActivateRegistration)
                SendActivationEmail(user, requestUri);

            var token = _membership.CreateTokenForUser(user);

			return new UserLogInTokenModel
			{
				key = WebAuthorizeConst.AuthorizeTokenName,
				value = token,
				userName = _membership.Name
			};
		}

		public void ActivateUser(string tokenString)
		{
			var decryptedToken = RSACrypto.Decrypt(tokenString);

			var userTokenValidation = new UserTokenValidation
			{
				Email = decryptedToken.Split(':')[0],
				ValidationCode = decryptedToken.Split(':')[1],
				UserId = long.Parse(decryptedToken.Split(':')[2])
			};

			if (userTokenValidation == null)
				throw new InvalidOperationException("Requested token is not valid");

			ActivateUser(userTokenValidation);
		}

		public void ApproveUser(long userId)
		{
			var user = _repository.Get<User>(userId);

			if (user == null)
				throw new BaseNotFoundException<User>();

			if (user.IsApproved)
				throw new InvalidOperationException("The requested user has already been approved");

			user.IsApproved = true;
			user.IsActive = true;

			_repository.Update(user);
			_repository.SaveChanges();

			SendUserApprovalNotificationEmail(user);
		}

		public void ActivateUser(UserTokenValidation userTokenValidation)
		{
			var user =
				_repository.First<User>(
					x => x.Email.Equals(userTokenValidation.Email) && (x.Id == userTokenValidation.UserId));

			if (user == null)
				throw new BaseNotFoundException<User>();

			if (string.IsNullOrEmpty(user.ConfirmationToken))
				throw new InvalidOperationException("Token expired");

			var decryptedToken = RSACrypto.Decrypt(user.ConfirmationToken);

			var userTokenValidationObj = new UserTokenValidation
			{
				Email = decryptedToken.Split(':')[0],
				ValidationCode = decryptedToken.Split(':')[1]
			};

			if (userTokenValidationObj.ValidationCode != userTokenValidation.ValidationCode)
				throw new InvalidOperationException("Validation code is not valid");

			if (!MembershipSetting.RegistrationApprovalMustBeDoneByAdmin)
				user.IsApproved = true;

			user.IsActive = true;
			user.ConfirmationToken = "";
			_repository.Update(user);
			_repository.SaveChanges();

			if (MembershipSetting.RegistrationApprovalMustBeDoneByAdmin)
				InformAdminForApproval(user);
		}

		public void ForgotPasswordRemind(string email, Uri baseRequestUrl)
		{
			var user = _repository.First<User>(x => x.Email == email);
			if ((user == null) || user.IsDeleted)
				throw new BaseNotFoundException<User>();


			var activationCode = StringExtensions.GenerateRandomCode(8);
			var nonEncryptedToken = $"{user.Email}:{activationCode}";
			var encryptedToken = RSACrypto.Encrypt(nonEncryptedToken);
			user.PasswordVerificationToken = encryptedToken;

			var expiryDays = MembershipSetting.ResetPasswordTokenExpiryIn;
			if (expiryDays == 0)
				expiryDays = 7;

			user.PasswordVerificationTokenExpirationDate = DateTime.Today.AddDays(expiryDays);
			_repository.Update(user);
			_repository.SaveChanges();

			SendResetPasswordEmailToUser(user, baseRequestUrl);
		}

		public void ResetPassword(ResetPasswordModel model)
		{
			var user = _repository.First<User>(x => x.Email == model.Email);
			if ((user == null) || user.IsDeleted)
				throw new BaseNotFoundException<User>();

			if ((user.PasswordVerificationToken != model.ResetPasswordToken) ||
				(user.PasswordVerificationTokenExpirationDate < DateTime.Today))
				throw new PasswordResetRequestExpiredException();

			_membership.ChangePassword(user, model.NewPassword);

			SendPasswordResetSuccessEmail(user);
		}

        public void SendEmailContactUs(EmailContactUsModel model)
        {
            SendEmailContactUsToAdministration(model);
        }

        public void SendEmailApplyCareer(EmailApplyCareerModel model)
        {
            SendEmailApplyCareerToAdministration(model);
        }


        public string ValidatePasswordToken(ValidateResetPasswordTokenModel model)
		{
			string decryptedToken;

			try
			{
				decryptedToken = RSACrypto.Decrypt(model.ResetPasswordToken);
			}
			catch
			{
				throw new AccessViolationException("Requested token is invalid");
			}

			var user = _repository.First<User>(x => x.PasswordVerificationToken == model.ResetPasswordToken);
			if ((user == null) || !decryptedToken.StartsWith(user.Email))
				throw new AccessViolationException("Requested token is invalid");

			if (user.PasswordVerificationTokenExpirationDate < DateTime.Today)
				throw new PasswordResetRequestExpiredException();

			return user.Email;
		}

		public void SendActivationEmail(User user, Uri requestUri)
		{
			var activationCode = StringExtensions.GenerateRandomCode(6);

			user.IsActive = false;
			var nonEncryptedToken = $"{user.Email}:{activationCode}:{user.Id}";
			var encryptedToken = String.Empty;
			try
			{
				encryptedToken = RSACrypto.Encrypt(nonEncryptedToken);
			}
			catch (Exception ex)
			{
				throw new Exception("Email: " + ex.Message);
			}

			user.ConfirmationToken = encryptedToken;

			SendActivationEmail(user, activationCode, encryptedToken, requestUri);
		}

		private void SendResetPasswordEmailToUser(User user, Uri baseUrl)
		{
			var adminEmailSetting =
				GeneralSiteSetting.AdminContactEmails.FirstOrDefault(x => x.IsDefaultToSendNotification);

			if (adminEmailSetting == null)
				throw new InvalidOperationException(AdminContactEmailNotConfiguredMessage);

			var model = new ForgotPasswordEmailTemplateModel
			{
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				ResetPasswordToken = user.PasswordVerificationToken,
				ResetPasswordUrl = new Uri(baseUrl, MembershipSetting.ResetPasswordPageUrl).ToString()
			};

			var emailTemplate = _templateService.ParseTemplate(model);



            notifyService.Services.notifyService _notificationService = new notifyService.Services.notifyService();


            // used MiddleName to save the external provider
            if (user.MiddleName.Contains("Facebook") || user.MiddleName.Contains("Google") || user.MiddleName.Contains("Apple") )
                _notificationService.SendEmailUserForgotPasswordExternalProvider(user.Email, user.FirstName, user.MiddleName);
            else
                _notificationService.SendEmailUserForgotPassword(user.Email, user.FirstName, user.LastName, model.ResetPasswordToken, model.ResetPasswordUrl);




            /*_notificationService.SendEmail(new[] { user.Email },
										   emailTemplate.TemplateTitle,
										   emailTemplate.TemplateContent,
										   adminEmailSetting.EmailAddress,
										   adminEmailSetting.DisplayName);*/
        }


        private void SendPasswordResetSuccessEmail(User user)
        {
            var adminEmailSetting =
                GeneralSiteSetting.AdminContactEmails.FirstOrDefault(x => x.IsDefaultToSendNotification);

            if (adminEmailSetting == null)
                throw new InvalidOperationException(AdminContactEmailNotConfiguredMessage);

            var model = new PasswordResetSuccessTemplateModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PasswordChangedDate = user.LastPasswordChangeDate?.ToString(DateShortFormat)
            };

            var emailTemplate = _templateService.ParseTemplate(model);

            /*_notificationService.SendEmail(new[] { user.Email },
										   emailTemplate.TemplateTitle,
										   emailTemplate.TemplateContent,
										   adminEmailSetting.EmailAddress,
										   adminEmailSetting.DisplayName);*/
        }

        private void SendEmailContactUsToAdministration(EmailContactUsModel emailInfo)
		{

            notifyService.Services.notifyService _notifService = new notifyService.Services.notifyService();

            _notifService.SendEmailContactUsToAdministration(emailInfo.Email, emailInfo.FirstName, emailInfo.LastName, emailInfo.PhoneNumber, emailInfo.Message);

        }

        private void SendEmailApplyCareerToAdministration(EmailApplyCareerModel emailInfo)
        {

            notifyService.Services.notifyService _notifService = new notifyService.Services.notifyService();

            _notifService.SendEmailToAdministrationApplyCareer(emailInfo.Email, emailInfo.FirstName, emailInfo.LastName, emailInfo.PhoneNumber,emailInfo.Address, emailInfo.City, emailInfo.State, emailInfo.codPostal,emailInfo.coverletter, emailInfo.openpositioncode);

        }
    

        private void SendActivationEmail(User user, string activationCode, string encryptedToken, Uri requestUri)
		{
			var adminEmailSetting =
				GeneralSiteSetting.AdminContactEmails.FirstOrDefault(x => x.IsDefaultToSendNotification);

			if (adminEmailSetting == null)
				throw new InvalidOperationException(AdminContactEmailNotConfiguredMessage);

			var targets = new List<string>
						  {
							  user.Email
						  };

			var modelInstance = new UserActivationEmailTemplateModel
			{
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				ActivationCode = activationCode,
				ActivationBaseUrl =
										new Uri(requestUri, _membershipSetting.UserActivationBaseUrl).ToString(),
				Token = encryptedToken
			};

			var emailTemplate = _templateService.ParseTemplate(modelInstance);



            notifyService.Services.notifyService  _notificationService = new notifyService.Services.notifyService();

            _notificationService.SendActivationEmailUserregistration(modelInstance.Email, modelInstance.FirstName, modelInstance.LastName, modelInstance.ActivationBaseUrl, modelInstance.ActivationCode, user.Id.ToString(), encryptedToken);

            /*_notificationService.SendEmail(targets,
										   emailTemplate.TemplateTitle,
										   emailTemplate.TemplateContent,
										   adminEmailSetting.EmailAddress,
										   adminEmailSetting.DisplayName);
                                           */
        }

		private void InformAdminForApproval(User user)
		{
			var adminEmailSetting =
				GeneralSiteSetting.AdminContactEmails.FirstOrDefault(x => x.IsDefaultToSendNotification);

			if (adminEmailSetting == null)
				throw new InvalidOperationException(AdminContactEmailNotConfiguredMessage);


			var modelInstance = new UserRegistrationApprovalPendingAdminNotificationTemplateModel
			{
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				PhoneNumber = user.CellPhoneNumber,
				HomePhoneNumber = user.HomePhoneNumber,
				RegistrationDate = user.RegistrationDate?.ToString(DateShortFormat)
			};

			var emailTemplate = _templateService.ParseTemplate(modelInstance);

			/*_notificationService.SendEmail(GeneralSiteSetting.AdminContactEmails.Select(x => x.EmailAddress).ToList(),
										   emailTemplate.TemplateTitle,
										   emailTemplate.TemplateContent,
										   adminEmailSetting.EmailAddress,
										   adminEmailSetting.DisplayName);*/
		}

		private void InformAdminForUserRegistered(User user)
		{
			var adminEmailSetting =
				GeneralSiteSetting.AdminContactEmails.FirstOrDefault(x => x.IsDefaultToSendNotification);

			if (adminEmailSetting == null)
				throw new InvalidOperationException(AdminContactEmailNotConfiguredMessage);


			var modelInstance = new UserRegistrationAdminNotificationTemplateModel
			{
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				PhoneNumber = user.CellPhoneNumber,
				HomePhoneNumber = user.HomePhoneNumber,
				RegistrationDate = user.RegistrationDate?.ToString(DateShortFormat)
			};

			var emailTemplate = _templateService.ParseTemplate(modelInstance);

			if (emailTemplate == null)
				return;

			/*_notificationService.SendEmail(GeneralSiteSetting.AdminContactEmails.Select(x => x.EmailAddress).ToList(),
										   emailTemplate.TemplateTitle,
										   emailTemplate.TemplateContent,
										   adminEmailSetting.EmailAddress,
										   adminEmailSetting.DisplayName);*/
		}

		private void SendUserApprovalNotificationEmail(User user)
		{
			var adminEmailSetting =
				GeneralSiteSetting.AdminContactEmails.FirstOrDefault(x => x.IsDefaultToSendNotification);

			if (adminEmailSetting == null)
				throw new InvalidOperationException(AdminContactEmailNotConfiguredMessage);

			var targets = new List<string>
						  {
							  user.Email
						  };

			var modelInstance = new UserRegistrationApprovedInformTemplateModel
			{
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				ApprovedDate = DateTime.Now.ToString(DateShortFormat)
			};

			var emailTemplate = _templateService.ParseTemplate(modelInstance);

			/*_notificationService.SendEmail(targets,
										   emailTemplate.TemplateTitle,
										   emailTemplate.TemplateContent,
										   adminEmailSetting.EmailAddress,
										   adminEmailSetting.DisplayName);*/
		}
	}
}