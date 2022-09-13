using System;
using System.IO;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace notifyService.Services.SendEmail
{
    internal class SendEmail
    {
        public static void SendEmailUserActivation(string useremail, string Firstname, string Lastname, string ActivationBaseUrl, string ActivationCode,string userid, string userTokenValidation)
        {
            ExecuteSendEmailUserActivation(useremail, Firstname, Lastname, ActivationBaseUrl, ActivationCode, userid, userTokenValidation).ToString();
        }

        public static void SendEmailUserForgotPassword(string useremail, string Firstname, string Lastname, string ResetPasswordToken, string ResetPasswordUrl)
        {
            ExecuteSendEmailUserForgotPassword(useremail, Firstname, Lastname, ResetPasswordToken, ResetPasswordUrl).ToString();
        }

        public static void SendEmailUserForgotPasswordExternalProvider(string useremail, string Firstname, string externalprovider)
        {
            ExecuteSendEmailUserForgotPasswordExternalProvider(useremail, Firstname, externalprovider).ToString();
        }

        public static void SendEmailContactUsToAdministration(string Email, string FirstName, string LastName, string PhoneNumber, string Message)
        {
            ExecuteSendEmailContactUsToAdministration(Email, FirstName, LastName, PhoneNumber, Message).ToString();
        }


        public static void SendEmailToAdministrationApplyCareer(string Email, string FirstName, string LastName, string PhoneNumber, string Address, string City, string State, string codPostal, string coverletter,string openpositioncode)
        {
            ExecuteSendEmailToAdministrationApplyCareer(Email, FirstName, LastName, PhoneNumber, Address, City, State, codPostal, coverletter, openpositioncode).ToString();
        }


        public static void SendEmailToVendorAddedNewProperty(string Email, string firstimageurl, string propertyname)
        {
            ExecuteSendEmailToVendorAddedNewProperty(Email, firstimageurl, propertyname).ToString();
        }


        static async Task ExecuteSendEmailUserActivation(string useremail, string Firstname, string Lastname, string ActivationBaseUrl, string ActivationCode,string userid, string userTokenValidation)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("noreply@roundcomb.com", "RoundComb");
            var subject = string.Concat("Welcome! Let's get started ",Firstname);
            //var to = new EmailAddress(useremail, string.Concat(Firstname + " " + Lastname));

            var to = new EmailAddress(useremail, string.Concat(Firstname + " " + Lastname));
            var plainTextContent = "";
            //var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";


            string strtemplatesPath = "C:\\Templates\\";


            string htmlContent = File.ReadAllText(string.Concat(strtemplatesPath, @"useractivationtemplate.html"));
            ActivationBaseUrl = ActivationBaseUrl.Replace("userActivation", "confirmuserregistration");
            ActivationBaseUrl = string.Concat(ActivationBaseUrl, "?userTokenValidation=", userTokenValidation);
     
            htmlContent = htmlContent.Replace("[&firstname]", Firstname).Replace("[&lastname]", Lastname);
            htmlContent = htmlContent.Replace("[&activationcode]", ActivationCode).Replace("[&url]", ActivationBaseUrl);


            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }

        static async Task ExecuteSendEmailUserForgotPassword(string useremail, string Firstname, string Lastname, string ResetPasswordToken, string ResetPasswordUrl)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("noreply@roundcomb.com", "RoundComb");
            var subject = string.Concat("RoundComb - Recover password request");
         
            var to = new EmailAddress(useremail, string.Concat(Firstname + " " + Lastname));
            var plainTextContent = "";
    
            string strtemplatesPath = "C:\\Templates\\";

            ResetPasswordUrl = ResetPasswordUrl.Replace("resetPassword", "forgotpasswordreset");
   
            ResetPasswordUrl = string.Concat(ResetPasswordUrl, "?resetpasswordToken=", ResetPasswordToken, "&Email=", useremail);
            string htmlContent = File.ReadAllText(string.Concat(strtemplatesPath, @"userrecoverpasswordtemplate.html"));
            htmlContent = htmlContent.Replace("[&firstname]", Firstname).Replace("[&url]", ResetPasswordUrl);

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }

        static async Task ExecuteSendEmailUserForgotPasswordExternalProvider(string useremail, string Firstname, string externalprovider)
        {
          var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("noreply@roundcomb.com", "RoundComb");
            var subject = string.Concat("RoundComb - Recover password request");

            var to = new EmailAddress(useremail, string.Concat(Firstname));
            var plainTextContent = "";

            string strtemplatesPath = "C:\\Templates\\";

            string htmlContent = File.ReadAllText(string.Concat(strtemplatesPath, @"userrecoverpasswordtemplateExternalProvider.html"));
            htmlContent = htmlContent.Replace("[&firstname]", Firstname).Replace("[&provider]", externalprovider);

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            
        }

        static async Task ExecuteSendEmailContactUsToAdministration(string Email, string FirstName, string LastName, string PhoneNumber, string Message)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("noreply@roundcomb.com", "RoundComb");
            var subject = string.Concat("RoundComb WebSite - Contact US");

            var to = new EmailAddress(Email, "RoundComb Administration");
            var plainTextContent = "";

            string strtemplatesPath = "C:\\Templates\\";

            string htmlContent = File.ReadAllText(string.Concat(strtemplatesPath, @"emailfromcontactUsToAdministration.html"));

            DateTime now = DateTime.Now;

            htmlContent = htmlContent.Replace("[&FirstName]", FirstName).Replace("[&LastName]", LastName).Replace("[&Email]", Email).Replace("[&Phonenumber]", PhoneNumber).Replace("[&Message]", Message).Replace("[&Date]", now.ToString("G", System.Globalization.CultureInfo.GetCultureInfo("en-US")));

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

        }

        static async Task ExecuteSendEmailToAdministrationApplyCareer(string Email, string FirstName, string LastName, string PhoneNumber, string Address, string City, string State, string codPostal, string coverletter,string openpositioncode)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("noreply@roundcomb.com", "RoundComb");
            var subject = string.Concat("RoundComb WebSite - Career Apply");

            var to = new EmailAddress(Email, "RoundComb Administration");
            var plainTextContent = "";

            string strtemplatesPath = "C:\\Templates\\";

            string htmlContent = File.ReadAllText(string.Concat(strtemplatesPath, @"emailfromCareersApply.html"));

            DateTime now = DateTime.Now;

            htmlContent = htmlContent.Replace("[&FirstName]", FirstName).Replace("[&LastName]", LastName).Replace("[&Email]", Email).Replace("[&Phonenumber]", PhoneNumber).Replace("[&Addres]", Address).Replace("[&City]", City).Replace("[&State]", State).Replace("[&CodePostal]", codPostal).Replace("[&CoverLetter]", coverletter).Replace("[&openpositioncode]", openpositioncode).Replace("[&Date]", now.ToString("G", System.Globalization.CultureInfo.GetCultureInfo("en-US")));

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

        }

        static async Task ExecuteSendEmailToVendorAddedNewProperty(string Email, string firstimageurl, string propertyname)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("noreply@roundcomb.com", "RoundComb");
            var subject = string.Concat("RoundComb WebSite - New Property");

            //Email = "bastos.m.luis@gmail.com";
            var to = new EmailAddress(Email, "RoundComb Administration");
            var plainTextContent = "";

            string strtemplatesPath = "C:\\Templates\\";

            string htmlContent = File.ReadAllText(string.Concat(strtemplatesPath, @"newpropertyaddedtemplate.html"));

            DateTime now = DateTime.Now;

            htmlContent = htmlContent.Replace("[&propertyname]", propertyname).Replace("[&propertyurl]", firstimageurl).Replace("[&Date]", now.ToString("G", System.Globalization.CultureInfo.GetCultureInfo("en-US")));

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

        }

    }
}