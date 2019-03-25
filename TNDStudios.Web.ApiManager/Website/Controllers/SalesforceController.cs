using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TNDStudios.Web.ApiManager.Controllers;
using TNDStudios.Web.ApiManager.Data.Salesforce;
using TNDStudios.Web.ApiManager.Data.Soap;

namespace Website.Controllers
{
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("1.1")]
    [Route("api/salesforce/test")]
    [ApiController]
    public class SalesforceController : SalesforceNotificationController<SalesforceObjectBase>
    {
        public override List<string> AllowedOrganisationIds { get; } = 
            new List<string>()
            {
                "00D80000000cDmQEAU"
            };

        public SalesforceController(ILogger<SalesforceNotificationController<SalesforceObjectBase>> logger)
            : base(logger)
        {
        }

        [NonAction]
        public override ActionResult<Boolean> Processor(
            List<SalesforceNotification<SalesforceObjectBase>> notifications)
        {
            return base.Processor(notifications);
        }
    }
}
