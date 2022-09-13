using System;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using Membership.Library.Implementation.OAuth.CernerHealth;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Owin.Security.Providers.GitHub;

[assembly: OwinStartup(typeof(Membership.Api.Startup))]


namespace Membership.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType("ExternalCookie");
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ExternalCookie",
                AuthenticationMode = AuthenticationMode.Passive,
                CookieName = ".AspNet.ExternalCookie",
                ExpireTimeSpan = TimeSpan.FromMinutes(5),
            });

            var options = new GitHubAuthenticationOptions
            {
                ClientId = "4e524b2cd3c75c889ce7",
                ClientSecret = "84940ca8e3c1ceeba380b16336bec5bd0dbaa049",
                Provider = new GitHubAuthenticationProvider
                {
                    OnAuthenticated = context =>
                    {
                        context.Identity.AddClaim(new Claim("urn:token:github", context.AccessToken));

                        return Task.FromResult(true);
                    }
                }
            };
            app.UseGitHubAuthentication(options);
               
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("GOOGLE_CLIENT_ID")))
            {
                app.UseGoogleAuthentication(
                    clientId: ConfigurationManager.AppSettings.Get("GOOGLE_CLIENT_ID"),
                    clientSecret: ConfigurationManager.AppSettings.Get("GOOGLE_CLIENT_SECRET"));
            }         

            app.Use(typeof(CernerHealthAuthenticationMiddleware), app, new CernerHealthAuthenticationOptions
            {
                ClientId = "your_app_id",
                ClientSecret = "your_app_secret"
            });

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
        }
    }
}              