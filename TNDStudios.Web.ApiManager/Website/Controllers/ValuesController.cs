using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TNDStudios.Web.ApiManager.Controllers;
using TNDStudios.Web.ApiManager.Security.Authentication;

namespace Website.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ManagedController
    {
        // GET api/values
        [Validate(Type: "admin", Category: "values", Permission: "read")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            CurrentUser?.Claims?.ForEach(claim => { });

            return new string[] { "value1", "value2" };
        }

        public ValuesController(ILogger<ManagedController> logger) : base(logger)
        {

        }        
    }
}
