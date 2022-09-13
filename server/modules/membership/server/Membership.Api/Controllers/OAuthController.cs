using Core.Extensions;
using Membership.Core;
using Membership.Core.Contracts;
using Membership.Core.Contracts.Common;
using Membership.Core.Dto;
using Membership.Library.Contracts;
using Membership.OAuthIntegration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace Membership.Api.Controllers
{
    public class OAuthController : ApiController
    {
        private readonly ICompanyService _companyService;
        //private readonly IJiraService _jiraService;
        private readonly IMembership _membership;
        private readonly IEnumerable<IOAuthServiceProvider> _oAuthServiceProviders;
        private readonly IUserService _userService;

        public OAuthController(IUserService userService,
                               IMembership membership,
                               //IJiraService jiraService,
                               ICompanyService companyService,
                               IEnumerable<IOAuthServiceProvider> oAuthServiceProviders)
        {
            _userService = userService;
            _membership = membership;
            //_jiraService = jiraService;
            _companyService = companyService;
            _oAuthServiceProviders = oAuthServiceProviders;
        }

        [HttpGet, Route("~/login/oauth/{externalLoginProvider}")]
        public IHttpActionResult OAuthLogIn(string externalLoginProvider)
        {
            var oauthServiceProvider =
                _oAuthServiceProviders.FirstOrDefault(x => x.ProviderName.Equals(externalLoginProvider));

            if (oauthServiceProvider == null)
                throw new InvalidOperationException("The provided service for OAuth login is not available");

            var callback = Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority +
                           "/membership.api/login/oauth/" + externalLoginProvider + "Callback";

            var url = oauthServiceProvider.GetAuthenticationUrl(callback);

            return Redirect(url.AbsoluteUri);
        }

        [HttpGet, Route("~/login/oauth/{externalLoginProvider}Callback")]
        public IHttpActionResult OAuthLogInCallback(string externalLoginProvider)
        {
            var oauthServiceProvider =
                _oAuthServiceProviders.FirstOrDefault(x => x.ProviderName.Equals(externalLoginProvider));

            if (oauthServiceProvider == null)
                throw new InvalidOperationException("The provided service for OAuth login is not available");

            var callback = Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority +
                           "/membership.api/login/oauth/" + externalLoginProvider + "Callback";

            var result = oauthServiceProvider.Authorize(Request.RequestUri, callback);

            if (result.StatusCode != HttpStatusCode.OK)
            {
                return
                    Redirect(Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority +
                             "/loginOAuthFailed?error=" + result.Status);
            }


            var validationResult = ProcessOAuthLoginResult(result.UserInfomation);

            var token = _membership.CreateToken();
            var redirectUrl = Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority + "/login?token=" + token;
            return Redirect(redirectUrl);
        }

        [HttpPost, Route("oauth/login")]
        public IHttpActionResult OAuthLogIn(OAuthUser result)
        {
            var validationResult = ProcessOAuthLoginResult(result);
            if (validationResult.IsSuccess)
            {
                var token = _membership.CreateToken();
                return Ok(new
                {
                    key = WebAuthorizeConst.AuthorizeTokenName,
                    value = token,
                    userName = _membership.Name
                });
            }
            throw new UnauthorizedAccessException();
        }

        private MembershipResult ProcessOAuthLoginResult(OAuthUser oAuthUser)
        {
            var userEntity = _userService.GetUserByEmail(oAuthUser.Email);
            UserBaseModel user;
            if (userEntity == null)
            {
                user = new UserBaseModel
                {
                    FirstName = oAuthUser.FirstName,
                    LastName = oAuthUser.LastName,
                    Email = oAuthUser.Email,
                    ExternalKey = oAuthUser.ExternalKey,
                    IsActive = true,
                    IsApproved = true,
                    Password = StringExtensions.GenerateRandomCode(10),
                    //TODO: How to define biz owner ??
                    BizOwnerId = _companyService.GetAll().First().Id
                };
                user = _userService.Register(user);
            }
            else
            {
                user = _userService.Get(userEntity.Id);
                user.ExternalKey = oAuthUser.ExternalKey;
                user.IsApproved = true;
                user.IsActive = true;
                _userService.Update(user);
            }

            return _membership.ValidateUser(user.Email, string.Empty, true);
        }

        /*[HttpGet]
        public IHttpActionResult Jira()
        {
            var callback = Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority +
                           "/membership.api/oauth/jiracallback";

            Uri url = _jiraService.GetAuthUrl(callback);

            return Redirect(url.AbsoluteUri);
        }

        [HttpGet]
        public IHttpActionResult JiraCallback()
        {
            var companyExternalId = "10011";

            var result = _jiraService.VerifyAuth(Request.RequestUri);
            if (result == null)
            {
                return Redirect(Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority);
            }

            var userInfo = _jiraService.MakeRequest("/rest/api/2/myself", "GET", result.AccessKey, result.SecretKey);

            UserBaseModel user;

            var userEntity = _userService.GetUserByEmail((string) userInfo.emailAddress);
            if (userEntity == null)
            {
                

                user = new UserBaseModel
<<<<<<< HEAD
                       {
                           FirstName = userInfo.displayName,
                           Email = userInfo.emailAddress,
                           ExternalKey = userInfo.key,
                           IsActive = false,
                           IsApproved = true,
                           Password = result.SecretKey,
                           BizOwnerId =
                               _companyService.GetAll()
                                              .Where(x => x.Code == companyExternalId)
                                              .Select(x => x.Id)
                                              .FirstOrDefault()
                       };
                user = _userService.Register(user);
=======
                {
                 //   FirstName = userInfo.displayName,
                    Email = userInfo.emailAddress,
                    ExternalKey = userInfo.key,
                    IsActive = false,
                    IsApproved = true,
                    Password = result.SecretKey,
                    BizOwnerId = this.companyService.GetAll().Where(x => x.Code == companyExternalId).Select(x => x.Id).FirstOrDefault()
                };
                var name = (string) userInfo.displayName;
                if (name.Contains(" "))
                {
                    user.FirstName = name.Split(new [] { ' ' }, 2).First();
                    user.LastName = name.Split(new[] { ' ' }, 2).Last();
                }
                else
                {
                    user.FirstName = userInfo.displayName;
                    user.LastName = string.Empty;
                }
                user = this.userService.Register(user);

                user.IsActive = false;
                this.userService.DeactivateUser(user.Id);
>>>>>>> mbac_new_membership
            }
            else
            {
                user = _userService.Get(userEntity.Id);
                user.Email = userInfo.emailAddress;
                user.ExternalKey = userInfo.key;
                user.BizOwnerId =
                    _companyService.GetAll()
                                   .Where(x => x.Code == companyExternalId)
                                   .Select(x => x.Id)
                                   .FirstOrDefault();
                _userService.Update(user);
            }

            _membership.ValidateUser(user.Email, string.Empty, true);

            // save tokens
            _userService.UpdateExternalToken(user.Id, result);

            return
                Redirect(Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority + "/login?token=" +
                         _membership.CreateToken());
        }*/
    }
}