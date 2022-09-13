
using notifyService.Interfaces;


namespace notifyService.Services
{
    public class notifyService : InotifyService
    {
        public void SendActivationEmailUserregistration(string useremail, string Firstname, string Lastname, string ActivationBaseUrl, string ActivationCode, string userid, string userTokenValidation)
        {
            SendEmail.SendEmail.SendEmailUserActivation(useremail, Firstname, Lastname, ActivationBaseUrl, ActivationCode, userid, userTokenValidation);
        }


        public void SendEmailUserForgotPassword(string useremail, string Firstname, string Lastname, string ResetPasswordToken, string ResetPasswordUrl)
        {
            SendEmail.SendEmail.SendEmailUserForgotPassword(useremail, Firstname, Lastname, ResetPasswordToken, ResetPasswordUrl);
        }

        public void SendEmailUserForgotPasswordExternalProvider(string useremail, string Firstname, string externalprovider)
        {
            SendEmail.SendEmail.SendEmailUserForgotPasswordExternalProvider(useremail, Firstname, externalprovider);
        }

        public void SendEmailContactUsToAdministration(string Email, string FirstName, string LastName, string PhoneNumber, string Message)
        {
            SendEmail.SendEmail.SendEmailContactUsToAdministration(Email, FirstName, LastName, PhoneNumber, Message);
        }


        public void SendEmailToAdministrationApplyCareer(string Email, string FirstName, string LastName, string PhoneNumber, string Address, string City, string State, string codPostal, string coverletter, string openpositioncode)
        {
            SendEmail.SendEmail.SendEmailToAdministrationApplyCareer(Email, FirstName, LastName, PhoneNumber, Address, City, State, codPostal, coverletter, openpositioncode);
        }

        public void SendEmailToVendorAddedNewProperty(string Email, string firstimageurl, string propertyname)
        {
            SendEmail.SendEmail.SendEmailToVendorAddedNewProperty(Email, firstimageurl, propertyname);
        }
    }
}