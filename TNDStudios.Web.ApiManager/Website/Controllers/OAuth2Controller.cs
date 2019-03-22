using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TNDStudios.Web.ApiManager.Controllers;

namespace Website.Controllers
{
    [Route("oauth2")]
    [ApiController]
    public class OAuth2Controller : OAuth2TokenController
    {
        public OAuth2Controller(ILogger<OAuth2Controller> logger)
            : base(logger)
        {
        }
    }
}