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
        public SalesforceController(ILogger<SalesforceNotificationController<SalesforceObjectBase>> logger) 
            : base(logger)
        {

        }
    }
}
