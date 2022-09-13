using System;
using System.Collections.Generic;
using System.Text;

namespace notifyService.Interfaces
{
    public interface InotifyService
    {

        void SendActivationEmailUserregistration(string useremail, string Firstname, string Lastname, string ActivationBaseUrl, string ActivationCode,string userid,string userTokenValidation);

        void SendEmailUserForgotPassword(string useremail, string Firstname, string Lastname, string ResetPasswordToken, string ResetPasswordUrl);

        void SendEmailUserForgotPasswordExternalProvider(string useremail, string Firstname, string externalprovider);

        void SendEmailContactUsToAdministration(string Email, string FirstName, string LastName, string PhoneNumber, string Message);

        void SendEmailToAdministrationApplyCareer(string Email, string FirstName, string LastName, string PhoneNumber, string Address, string City, string State, string codPostal, string coverletter, string openpositioncode);

        void SendEmailToVendorAddedNewProperty(string Email, string firstimageurl, string propertyname);
    }
}
