using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TNDStudios.Web.ApiManager.Data.Salesforce;
using TNDStudios.Web.ApiManager.Data.Soap;

namespace TNDStudios.Web.ApiManager.Controllers
{
    public class SalesforceNotificationController<T> : ManagedController 
        where T: SalesforceObjectBase, new()
    {
        /// <summary>
        /// List of organisation Id' that will be allowed to post data to this controller
        /// </summary>
        public virtual List<String> AllowedOrganisationIds { get; } = new List<String>();

        public SalesforceNotificationController(ILogger logger) : base(logger)
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
        public ActionResult<Boolean> Post([FromBody]SoapMessage<SalesforceNotificationsBody<T>> message)
            => ValidateRequest(message.Envelope.Body.Notifications) ? 
                Processor(message.Envelope.Body.Notifications.Items) : 
                new UnauthorizedObjectResult(false);

        /// <summary>
        /// Check to see if the organisation Id's in the valid organisation id's list match
        /// </summary>
        /// <param name="notifications">The header</param>
        /// <returns>If the message is a valid one</returns>
        [NonAction]
        public virtual Boolean ValidateRequest(SalesforceNotifications<T> notifications)
            => AllowedOrganisationIds.Contains(notifications.OrganizationId);

        /// <summary>
        /// Process the notifications to be overloaded for each implementation
        /// </summary>
        /// <param name="notifications">The list of notifications from the notifications portion of the message</param>
        /// <returns>If the messages could be processed or not</returns>
        [NonAction]
        public virtual ActionResult<Boolean> Processor(List<SalesforceNotification<T>> notifications)
        {
            return new OkObjectResult(true);
        }
    }
}
