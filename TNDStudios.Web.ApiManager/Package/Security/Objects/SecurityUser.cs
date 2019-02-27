using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TNDStudios.Web.ApiManager.Security.Objects
{
    /// <summary>
    /// A user object specifically for defining the security context
    /// of a potential user login
    /// </summary>
    [JsonObject]
    public class SecurityUser
    {
        [JsonProperty]
        public String Id { get; set; }

        [JsonProperty]
        public String Key { get; set; }

        [JsonProperty]
        public List<String> Authentication { get; set; }

        [JsonProperty]
        public String Username { get; set; }

        [JsonProperty]
        public String Password { get; set; }

        [JsonProperty]
        public List<SecurityClaim> Claims { get; set; }
    }
}
