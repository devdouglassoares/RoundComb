using Core.SiteSettings;
using Membership.FacebookOAuthIntegration.Settings;
using Membership.OAuthIntegration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;

namespace Membership.FacebookOAuthIntegration
{
    public class FacebookOAuthServiceProvider : IOAuthServiceProvider
    {
        private readonly ISiteSettingService _siteSettingService;
        private string _appId;
        private string _appSecret;

        public const string TokenRequestUrl = "https://graph.facebook.com/oauth/access_token?client_id={0}&redirect_uri={1}&client_secret={2}&code={3}";
        public const string EmailRequestUrl = "https://graph.facebook.com/me?access_token={0}&fields=first_name,last_name,email";

        public string ProviderName => "facebook";

        public FacebookOAuthServiceProvider(ISiteSettingService siteSettingService)
        {
            _siteSettingService = siteSettingService;
        }

        private void GeFacebookSettings()
        {
            var facebookAppSetting = _siteSettingService.GetSetting<FacebookOAuthSetting>();

            if (string.IsNullOrEmpty(facebookAppSetting.AppId) || string.IsNullOrEmpty(facebookAppSetting.AppId))
                throw new InvalidOperationException("Facebook OAuth configuration missing");

            _appId = facebookAppSetting.AppId;
            _appSecret = facebookAppSetting.AppSecret;
        }

        public Uri GetAuthenticationUrl(string callbackUrl)
        {
            GeFacebookSettings();
            var absoluteUrl =
                $"https://www.facebook.com/dialog/oauth?client_id={_appId}&redirect_uri={callbackUrl}&scope=email,user_about_me,public_profile";

            return new Uri(absoluteUrl);
        }

        public OAuthLogInResponse Authorize(Uri requestUri, string originalRedirectUrl)
        {
            var code = requestUri.ParseQueryString().Get("code");

            var response = new OAuthLogInResponse();

            if (string.IsNullOrEmpty(code))
            {
                response.Status = "Invalid Code";
                response.StatusCode = HttpStatusCode.Unauthorized;
                return response;
            }
            GeFacebookSettings();

            var token = GetAccessToken(code, _appId, _appSecret, originalRedirectUrl);
            if (string.IsNullOrEmpty(token))
            {
                response.Status = "Invalid Token";
                response.StatusCode = HttpStatusCode.Unauthorized;
                return response;
            }

            var accountInfo = GetAccountInfo(token);

            if (accountInfo != null)
            {
                response.StatusCode = HttpStatusCode.OK;
                response.UserInfomation = new OAuthUser
                                          {
                                              Email = accountInfo.email,
                                              FirstName = accountInfo.first_name,
                                              LastName = accountInfo.last_name,
                                              ExternalKey = accountInfo.id
                                          };
            }
            else
            {
                response.Status = "Error while getting account info";
                response.StatusCode = HttpStatusCode.InternalServerError;
            }
            return response;
        }

        #region Private Methods
        private string GetAccessToken(string code, string clientId, string clientSecret, string originalRedirectUrl)
        {
            try
            {
                var server = HttpContext.Current.Server;
                var redirectUrl = originalRedirectUrl;

                var url = string.Format(TokenRequestUrl,
                                        server.UrlEncode(clientId),
                                        server.UrlEncode(redirectUrl),
                                        server.UrlEncode(clientSecret),
                                        server.UrlEncode(code));

                var wr = WebRequest.Create(url);

                wr.Method = "GET";
                var wres = wr.GetResponse();
                using (var stream = wres.GetResponseStream())
                using (var sr = new StreamReader(stream))
                {
                    var result = HttpUtility.ParseQueryString(sr.ReadToEnd());
                    return result["access_token"];
                }
            }
            catch (Exception ex)
            {
                var wex = ex as WebException;
                string error = null;
                if (wex != null && wex.Response != null)
                {
                    using (var stream = wex.Response.GetResponseStream())
                    {
                        if (stream != null)
                            using (var sr = new StreamReader(stream))
                                error = sr.ReadToEnd();
                    }
                    return error;
                }
            }
            return null;
        }

        private FacebookUserInformation GetAccountInfo(string token)
        {
            try
            {
                var wr = WebRequest.Create(string.Format(EmailRequestUrl, token));
                wr.Method = "GET";

                var wres = wr.GetResponse();
                using (var stream = wres.GetResponseStream())
                {
                    var sr = new StreamReader(stream);
                    var input = sr.ReadToEnd();

                    var result = JsonConvert.DeserializeObject<FacebookUserInformation>(input);

                    return result;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        #endregion
    }
}