using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TNDStudios.Web.ApiManager.Security.Objects;

namespace TNDStudios.Web.ApiManager.Controllers
{
    /// <summary>
    /// Managed controller to handle passing context of the user etc. to the 
    /// controllers that inherit it 
    /// </summary>
    public class ManagedController : ControllerBase
    {
        public SecurityUser CurrentUser
        {
            get
            {
                // By default the result is a failure (null) indicating the user is not valid
                SecurityUser contextUser = null;

                // Attempt to get the raw json from the session for the current user
                var rawData = Request.HttpContext.Session.GetString("CurrentUser");

                // If there is some data to transform for the current user otherwise it stays as a fail state
                if (rawData != null)
                    contextUser = JsonConvert.DeserializeObject<SecurityUser>(rawData); // Cast the user json to the correct object type

                // Return the user that was stored in the user context session
                return contextUser;
            }
        }
    }
}
