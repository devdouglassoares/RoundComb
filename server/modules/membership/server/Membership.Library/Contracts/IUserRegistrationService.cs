using Core;
using Membership.Core.Dto;
using Membership.Core.Entities;
using Membership.Core.Models;
using Membership.Library.Dto;
using System;

namespace Membership.Library.Contracts
{
    public interface IUserRegistrationService : IDependency
    {
        UserLogInTokenModel Register(UserRegistrationModel model, Uri requestUri);

        void ActivateUser(UserTokenValidation userTokenValidation);

        void ActivateUser(string tokenString);

        void ApproveUser(long userId);

        void ForgotPasswordRemind(string email, Uri baseRequestUrl);

        void ResetPassword(ResetPasswordModel model);

        void SendEmailContactUs(EmailContactUsModel model);

        void SendEmailApplyCareer(EmailApplyCareerModel model);

        string ValidatePasswordToken(ValidateResetPasswordTokenModel model);

        void SendActivationEmail(User user, Uri requestUri);
    }
}