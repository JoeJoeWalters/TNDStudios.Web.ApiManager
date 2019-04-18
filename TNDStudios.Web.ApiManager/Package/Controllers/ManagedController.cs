using TNDStudios.Web.ApiManager.Security.Objects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Linq;
using System;

namespace TNDStudios.Web.ApiManager.Controllers
{
    /// <summary>
    /// Managed controller to handle passing context of the user etc. to the 
    /// controllers that inherit it 
    /// </summary>
    public class ManagedController : ControllerBase
    {
        public ILogger Logger { get; set; }

        public ManagedController(ILogger logger)
            => Logger = logger;

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
                            User.FindFirst(claim => {return (claim.Type == ClaimTypes.AuthenticationMethod); })?.Value
                                        .Split(",")
                                        .Select(item => (SecurityUser.AuthenticationType)Enum.Parse(typeof(SecurityUser.AuthenticationType), item))
                                        .ToList(),
                        UserClaims = User.Claims
                                        .Where(claim => !claim.Type.ToLower().StartsWith("security:"))
                                        .ToList<Claim>(),
                        SecurityClaims = User.Claims
                                        .Where(claim => claim.Type.ToLower().StartsWith("security:"))
                                        .Select(claim => new Claim(claim.Type.Replace("security:", String.Empty), claim.Value))
                                        .ToList<Claim>()
                    };
                    
                    // Return the user that was stored in the user context session
                    return contextUser;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }
}
