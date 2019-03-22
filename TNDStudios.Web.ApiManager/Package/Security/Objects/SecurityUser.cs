using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TNDStudios.Web.ApiManager.Security.Objects
{
    /// <summary>
    /// A user object specifically for defining the security context
    /// of a potential user login
    /// </summary>
    [JsonObject]
    public class SecurityUser
    {
        [JsonProperty(Required = Required.Always)]
        public String Id { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public String Key { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public String ClientId { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public String ClientSecret { get; set; }

        [JsonProperty(Required = Required.Always)]
        public List<String> Authentication { get; set; }

        [JsonProperty(Required = Required.Always)]
        public String Username { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public String Password { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public List<SecurityClaim> Claims { get; set; }
    }
}
