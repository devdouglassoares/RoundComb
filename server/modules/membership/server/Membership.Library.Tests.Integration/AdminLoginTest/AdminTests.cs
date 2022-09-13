using System;
using System.Net;
using Core.CastleWindsorIntegration.IoC;
using Core.IoC;
/*
using NUnit.Framework;
using RestSharp;
*/
namespace Membership.Library.Tests.Integration.AdminLoginTest
{
    //[TestFixture]
    public class AdminTests
    {/*
        [SetUp]
        public void SetUp()
        {
            ApplicationContainer.CreateApplicationContainer(new CastleWindsorConfig());
        }
        
        [Description("Membership.api should create user / login / logout / delete user")]
        [Test]
        
        public void Membership_should_create_login_logout_delete()
        {
            var url = "http://roundcomb.com";
            var restApiLoginAdmin = new RestClient(url + "/membership.api/auth");
            var loginAdminCall = new RestRequest();
            loginAdminCall.Method = Method.POST;
            loginAdminCall.AddParameter("Login", "admin");
            loginAdminCall.AddParameter("Password", "qwerty");

            var responseAdminLogin = restApiLoginAdmin.Execute(loginAdminCall);
            var responseAdminLoginContent = responseAdminLogin.Content;
            Assert.AreEqual(responseAdminLogin.StatusCode, HttpStatusCode.OK);


            var restApiClient = new RestClient(url + "/membership.api/bizUsers/post");
            var createUserCall = new RestRequest();

            createUserCall.Method = Method.POST;
            createUserCall.AddParameter("FirstName", "TestUserFirstName");
            createUserCall.AddParameter("LastName", "TestUserLastName");
            createUserCall.AddParameter("Email", "testuser@testuser.com");
            createUserCall.AddParameter("Password", "123123");
            var token =
                responseAdminLoginContent.Substring(
                    responseAdminLoginContent.IndexOf("value", StringComparison.Ordinal),
                    responseAdminLoginContent.Length - responseAdminLoginContent.IndexOf("value", StringComparison.Ordinal) -1);
            createUserCall.AddHeader("AuthToken", token.Substring(8, token.Length - 9));
            var responseCreate = restApiClient.Execute(createUserCall);
            var createContent = responseCreate.Content; // raw content as string  
            var testUserId = createContent.Substring(6, createContent.Length - 7);
            Assert.AreEqual(responseCreate.StatusCode, HttpStatusCode.OK);

            var restApiLogin = new RestClient(url + "/membership.api/auth");
            var loginCall = new RestRequest();
            loginCall.Method = Method.POST;
            loginCall.AddParameter("Login", "testuser@testuser.com");
            loginCall.AddParameter("Password", "123123");

            var responseLogin = restApiLogin.Execute(loginCall);
            var responseLoginContent = responseLogin.Content;
            var userToken =
                responseAdminLoginContent.Substring(
                    responseAdminLoginContent.IndexOf("value", StringComparison.Ordinal),
                    responseAdminLoginContent.Length - responseAdminLoginContent.IndexOf("value", StringComparison.Ordinal) - 1);
            Assert.AreEqual(responseLogin.StatusCode, HttpStatusCode.OK);


            var restApiDeleteUser = new RestClient(url + "/membership.api/bizUsers/" + testUserId);
            var deleteCall = new RestRequest();
            deleteCall.Method = Method.DELETE;
            deleteCall.AddHeader("AuthToken", token.Substring(8, token.Length - 9));

            var responseDeleteUser = restApiDeleteUser.Execute(deleteCall);

            Assert.AreEqual(responseDeleteUser.StatusCode, HttpStatusCode.OK);
            
    }
    */
    }
}