using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using TNDStudios.Web.ApiManager.Security.OAuth;

namespace TNDStudios.Web.ApiManager.Controllers
{
    public class OAuth2TokenController : ControllerBase
    {
        public ILogger Logger { get; set; }

        public OAuth2TokenController(ILogger logger)
            => Logger = logger;

        [Consumes(@"application/json")]
        [Produces(@"application/json")]
        [ResponseCache(NoStore = true)]
        [HttpPost]
        public virtual ActionResult Post(OAuthTokenRequest request)
        {
            if ((new Random()).NextDouble() > (Double)0.5)
                return new OkObjectResult(
                    new OAuthTokenSuccess()
                    {
                        AccessToken = Guid.NewGuid().ToString(),
                        ExpiresIn = 3600,
                        RefreshToken = Guid.NewGuid().ToString(),
                        Scope = "test",
                        TokenType = "bearer"
                    });
            else
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
