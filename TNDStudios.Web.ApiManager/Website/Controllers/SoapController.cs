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
    public class SoapController : SoapManagedController
    {
        public SoapController(ILogger<SoapManagedController> logger) : base(logger)
        {

        }

        /// <summary>
        /// Provide an endpoint to recieve a soap notification message
        /// in a structure that recognises notifications from Salesforce
        /// </summary>
        /// <param name="message">The translated Soap request as an object</param>
        /// <returns>A success or failure response</returns>
        [Consumes(@"application/soap+xml", otherContentTypes: @"text/xml")]
        [HttpPost]
        public Boolean Post([FromBody]SoapMessage<SalesforceNotificationsBody<SalesforceObjectBase>> message)
        {
            //message.Envelope.Body.Notifications.Items[0].SalesforceObject;
            return true;
        }
    }
}
