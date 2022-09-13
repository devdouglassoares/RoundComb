//using System;
//using System.Configuration;
//using System.Security.Cryptography;
//using System.Text;
//using DotNetAuth.OAuth1a;
//using Membership.Core.Entities;
//using Membership.Library.Contracts;
//using Membership.Library.Entities;
//using Newtonsoft.Json;
//using RestSharp;

//namespace Membership.Library.Services
//{
//    public class JiraService : IJiraService
//    {
//        private ApplicationCredentials JiraApplicationCredentials;
//        private JIRAOAuth1aProvider JiraOAuth1AProvider;
//        private OAuth10aStateManager OAuth10AStateManager;

//        private string JiraBaseUrl = ConfigurationManager.AppSettings["JiraBaseUrl"];

//        public JiraService()
//        {
//            JiraApplicationCredentials = new ApplicationCredentials
//            {
//                ConsumerKey = "mymagviewconsumerkey",
//                ConsumerSecret = @"<RSAKeyValue><Modulus>tGIwsCH2KKa6vxUDupW92rF68S5SRbgr7Yp0xeadBsb0BruTt4GMrVL7QtiZWM8qUkY1ccMa7LkXD93uuNUnQEsH65s8ryID9PDeEtCBcxFEZFdcKfyKR+5B+NRLW5lJq10sHzWbJ0EROUmEjoYfi3CtsMkJHYHDL9dZeCqAZHM=</Modulus><Exponent>AQAB</Exponent><P>14DdDg26CrLhAFQIQLT1KrKVPYr0Wusi2ovZApz2/RnM7a7CWUJuDR3ClW5g9hdi+KQ0ceD5oJYX5Vexv2uk+w==</P><Q>1kfU0+DkXc6I/jXHJ6pDLA5s7dBHzWgDsBzplSdkVQbKT3MbeYjeByOxzXhulOWLBQW/vxmW4HwU95KTRlj06Q==</Q><DP>SPoBYY3yb0cN/J94P/lHgJMDCNkyUEuJ/PoYndLrrN/8zow8kh91xwlJ6HJ9cTiQMmTgwaOOxPuu0eI1df4M2w==</DP><DQ>klJaspRPXP877NssM5nAZMU0/O/NGCZ+3jPgDUno6WbJn5cqm8MqWhW1xGkImgRk+fkDBquiq4gPiT898jusgQ==</DQ><InverseQ>d5Zrr6Q8AO/0isr/3aa6O6NLQxISLKcPDk2NOccAfS/xOtfOz4sJYM3+Bs4Io9+dZGSDCA54Lw03eHTNQghS0A==</InverseQ><D>WFlbZXlM2r5G6z48tE+RTKLvB1/btgAtq8vLw/5e3KnnbcDD6fZO07m4DRaPjRryrJdsp8qazmUdcY0O1oK4FQfpprknDjP+R1XHhbhkQ4WEwjmxPstZMUZaDWF58d3otc23mCzwh3YcUWFu09KnMpzZsK59OfyjtkS44EDWpbE=</D></RSAKeyValue>",
//            };
//            JiraOAuth1AProvider = new JIRAOAuth1aProvider(JiraBaseUrl);
//            OAuth10AStateManager = new OAuth10aStateManager((k, v) => { }, k => (string)null);
//        }

//        public dynamic MakeRequest(string url, string method, string accessToken, string accessTokenSecret)
//        {

//            var provider = JiraOAuth1AProvider;
//            var jiraCredentials = JiraApplicationCredentials;

//            var http = new Http { Url = new Uri(JiraBaseUrl + url) };
//            // RestSharp.RestRequest is not supported yet. So the following ApplyAccessToken is only available for RestSharp.Http
//            http.ApplyAccessTokenToHeader(provider, jiraCredentials, accessToken, accessTokenSecret, method);
//            var response = http.Get();
//            var content = JsonConvert.DeserializeObject<dynamic>(response.Content);

//            return content;
//        }

//        public Uri GetAuthUrl(string callbackUrl)
//        {
//            var authorizationUri = OAuth1aProcess.GetAuthorizationUri(JiraOAuth1AProvider, JiraApplicationCredentials,
//                                                                      callbackUrl, OAuth10AStateManager);
            
//            authorizationUri.Wait();

//            return authorizationUri.Result;
//        }

//        public UserExternalLogin VerifyAuth(Uri requestUri)
//        {
//            var processUserResponse = OAuth1aProcess.ProcessUserResponse(JiraOAuth1AProvider, JiraApplicationCredentials,
//                                                                      requestUri, OAuth10AStateManager);
//            processUserResponse.Wait();
//            var accessToken = processUserResponse.Result.AllParameters["oauth_token"];
//            var accessTokenSecret = processUserResponse.Result.AllParameters["oauth_token_secret"];

//            if (accessToken == null || accessTokenSecret == null)
//            {
//                return null;
//            }

//            return new UserExternalLogin
//            {
//                ExternalProviderName = "Jira",
//                SecretKey = accessTokenSecret,
//                AccessKey = accessToken
//            };
//        }


//        public class JIRAOAuth1aProvider : OAuth1aProviderDefinition
//        {
//            public JIRAOAuth1aProvider(string JIRA_BASE_URL)
//            {
//                RequestTokenEndopointUri = JIRA_BASE_URL + "/plugins/servlet/oauth/request-token";
//                AuthorizeEndpointUri = JIRA_BASE_URL + "/plugins/servlet/oauth/authorize";
//                AuthenticateEndpointUri = JIRA_BASE_URL + "/plugins/servlet/oauth/authorize";
//                AccessTokenEndpointUri = JIRA_BASE_URL + "/plugins/servlet/oauth/access-token";
//            }
//            public override string GetSignatureMethod()
//            {
//                return "RSA-SHA1";
//            }
//            public override string Sign(string stringToSign, string signingKey)
//            {
//                var privateKey = new RSACryptoServiceProvider();
//                privateKey.FromXmlString(signingKey);

//                var shaHashObject = new SHA1Managed();
//                var data = Encoding.UTF8.GetBytes(stringToSign);
//                var hash = shaHashObject.ComputeHash(data);

//                var signedValue = privateKey.SignHash(hash, CryptoConfig.MapNameToOID("SHA1"));

//                var result = Convert.ToBase64String(signedValue, Base64FormattingOptions.None);
//                return result;
//            }
//        }
//    }
//}