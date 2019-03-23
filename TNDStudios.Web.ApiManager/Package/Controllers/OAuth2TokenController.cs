using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TNDStudios.Web.ApiManager.Security.Authentication;
using TNDStudios.Web.ApiManager.Security.OAuth;
using TNDStudios.Web.ApiManager.Security.Objects;

namespace TNDStudios.Web.ApiManager.Controllers
{
    // https://stackoverflow.com/questions/50992380/oauth2-client-authentication-with-multiple-users
    public class OAuth2TokenController : ControllerBase
    {
        public ILogger Logger { get; set; }

        public IUserAuthenticator Authenticator { get; set; }

        public OAuth2TokenController(ILogger logger, IUserAuthenticator userAuthenticator)
        {
            Logger = logger;
            Authenticator = userAuthenticator;
        }

        [Consumes(@"application/json")]
        [Produces(@"application/json")]
        [ResponseCache(NoStore = true)]
        [HttpPost]
        public virtual ActionResult Post(OAuthTokenRequest request)
        {
            // Check the client id and secret being asked for;
            SecurityUser securityUser = 
                Authenticator.AuthenticateOAuth(request).Result;
            
            if (securityUser != null)
            {
                return new OkObjectResult(
                    new OAuthTokenSuccess()
                    {
                        AccessToken = securityUser.Key,
                        ExpiresIn = 3600,
                        RefreshToken = securityUser.Key,
                        Scope = "test",
                        TokenType = "bearer"
                    });
            }
            else
            {
                return new BadRequestObjectResult(
                    new OAuthTokenFailure()
                    {
                        Reason = OAuthTokenFailure.ReasonType.unauthorized_client,
                        ReasonDescription = "Reason for the failure here",
                        ReasonUri = "url of the failure code"
                    });
            }
        }
    }
}
