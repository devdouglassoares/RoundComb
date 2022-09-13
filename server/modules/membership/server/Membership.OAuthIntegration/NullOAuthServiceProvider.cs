using System;

namespace Membership.OAuthIntegration
{
    /// <summary>
    /// Fake class for injecting into controller
    /// </summary>
    public class NullOAuthServiceProvider : IOAuthServiceProvider
    {
        public string ProviderName => "NullOAuthServiceProvider";

        public Uri GetAuthenticationUrl(string callbackUrl)
        {
            throw new NotImplementedException();
        }

        public OAuthLogInResponse Authorize(Uri requestUri, string originalRedirectUrl)
        {
            throw new NotImplementedException();
        }
    }
}