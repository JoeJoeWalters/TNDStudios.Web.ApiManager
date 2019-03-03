using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            Logger.LogInformation("This is a message");

            return new string[] { "value1", "value2" };
        }
        
    }
}
