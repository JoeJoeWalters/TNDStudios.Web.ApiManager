using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TNDStudios.Web.ApiManager.Security.Objects
{
    /// <summary>
    /// The "claim" of what a security check is needed or what a 
    /// user can claim they can do.
    /// </summary>
    [JsonObject]
    public class SecurityClaim
    {
        [JsonProperty]
        public String Name { get; set; }

        [JsonProperty]
        public List<String> Companies { get; set; }

        [JsonProperty]
        public List<String> Permissions { get; set; }
    }
}
