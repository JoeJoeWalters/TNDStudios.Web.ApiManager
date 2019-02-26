using Microsoft.AspNetCore.Mvc;
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
            get => new SecurityUser()
            {
                Claims = new List<SecurityClaim>()
                {
                    new SecurityClaim()
                    {
                        Companies = new List<String> { "ba" },
                        Name = "admin",
                        Permissions = new List<String>(){ "read", "write", "delete" }
                    }
                }
            };
        }
    }
}
