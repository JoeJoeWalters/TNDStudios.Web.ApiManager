using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using TNDStudios.Web.ApiManager.Security;
using TNDStudios.Web.ApiManager.Security.Objects;
using TNDStudios.Web.ApiManager.Extensions;

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
                try
                {
                    // By default the result is a failure (null) indicating the user is not valid
                    SecurityUser contextUser = new SecurityUser()
                    {
                        Id = User.FindFirst(claim => { return (claim.Type == ClaimTypes.NameIdentifier); })?.Value,
                        Username = User.FindFirst(claim => { return (claim.Type == ClaimTypes.Name); })?.Value,
                        Key = User.FindFirst(claim => { return (claim.Type == ClaimTypes.Sid); })?.Value,
                        Authentication =
                            User.FindFirst(claim => { return (claim.Type == ClaimTypes.AuthenticationMethod); })?.Value
                            .Split(",").ToList<String>(),
                        Claims = User.Claims.ToList<Claim>().ToSecurityClaims()
                    };
                    
                    // Return the user that was stored in the user context session
                    return contextUser;
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
