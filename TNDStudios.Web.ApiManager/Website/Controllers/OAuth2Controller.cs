using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using TNDStudios.Web.ApiManager.Controllers;
using TNDStudios.Web.ApiManager.Security.Authentication;

namespace Website.Controllers
{
    [Route("oauth2")]
    [ApiController]
    public class OAuth2Controller : OAuth2TokenController
    {
        private static IUserAuthenticator userAuthenticator;

        static OAuth2Controller()
        {
            // Set up the authentication service with the appropriate authenticator implementation
            FileStream accessControl = System.IO.File.OpenRead(Path.Combine(Environment.CurrentDirectory, "users.json"));
            userAuthenticator = new UserAuthenticator();
            userAuthenticator.RefreshAccessList(accessControl);
        }

        public OAuth2Controller(ILogger<OAuth2Controller> logger)
            : base(logger, userAuthenticator, Startup.JWTKey, Startup.JWTIssuer, Startup.JWTAudience)
        {
        }
    }
}