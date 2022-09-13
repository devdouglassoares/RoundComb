using Core;
using System;

namespace Membership.OAuthIntegration
{
    public interface IOAuthServiceProvider: IDependency
    {
        string ProviderName { get; }

        Uri GetAuthenticationUrl(string callbackUrl);

        OAuthLogInResponse Authorize(Uri requestUri, string originalRedirectUrl);
    }
}