using Microsoft.Identity.Client; // MSAL.NET
using Microsoft.Rest;
using System;
using System.Linq;
using System.Security;

namespace PowerBIEmbedding
{
    public class Authentication
    {
        // App represents the app for which the token is received from Azure AD
        private static IPublicClientApplication app = null;

        public static TokenCredentials GetTokenCredentials()
        {
            // For further information:
            // Tutorial: Enable your Web Apps to sign-in users and call APIs with the 
            // Microsoft identity platform for developers
            // https://github.com/Azure-Samples/ms-identity-aspnetcore-webapp-tutorial

            string token = null;

            // Build an app, if not already built
            if (app == null)
            {
                app = PublicClientApplicationBuilder.Create(Configurations.ApplicationId)
                    .WithAuthority(Configurations.AuthorityUrl)
                    .Build();
            }

            // Acquire a token with the recommended pattern
            try
            {
                // First, attempt to retrieve an existing token from the cache
                var accounts = app.GetAccountsAsync()
                    .GetAwaiter()
                    .GetResult();
                var account = accounts.FirstOrDefault();
                var res = app.AcquireTokenSilent(new[] { Configurations.ResourceUrl + "/.default" }, account)
                    .ExecuteAsync()
                    .GetAwaiter()
                    .GetResult();

                token = res?.AccessToken;
            }
            // If retrieval fails, acquire a new token by using the Master App user name and password
            catch (MsalUiRequiredException ex)
            {
                // Store the password as a secure string
                var securePassword = new SecureString();
                foreach (char c in Configurations.MasterAppPassword)
                {
                    securePassword.AppendChar(c);
                }

                var res = app.AcquireTokenByUsernamePassword(new[] { Configurations.ResourceUrl + "/.default" }, Configurations.MasterAppUsername, securePassword)
                    .WithClaims(ex.Claims)
                    .ExecuteAsync()
                    .GetAwaiter()
                    .GetResult();

                token = res?.AccessToken;
            }
            catch (Exception)
            {
                // Log the exception
            }

            return new TokenCredentials(token, "Bearer");
        }
    }
}