using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TNDStudios.Web.ApiManager.Controllers;
using TNDStudios.Web.ApiManager.Security.Authentication;

namespace Website.Controllers
{
    [Authorize]
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("1.1")]
    [Route("api/values")]
    [ApiController]
    public class ValuesController : ManagedController
    {
        // GET api/values
        [Authorize(Policy = "AdminOnly")]
        [HttpGet, MapToApiVersion("1.0"), MapToApiVersion("1.1")]
        public ActionResult<IEnumerable<string>> Get()
        {
            // Check we have full admin access not just partial
            if (!User.HasClaim("admin", "full"))
                return new UnauthorizedResult();

            return new string[] { "value1", "value2" };
        }

        public ValuesController(ILogger<ManagedController> logger) : base(logger)
        {

        }        
    }
}
