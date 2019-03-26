using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore, MemberSerialization = MemberSerialization.OptOut)]
    public class SecurityUser
    {
        public enum AuthenticationType
        {
            basic,
            oauth,
            apikey
        }

        [JsonProperty(Required = Required.Always)]
        public String Id { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public String Key { get; set; } = String.Empty;

        [JsonProperty(Required = Required.Default)]
        public String ClientId { get; set; } = String.Empty;

        [JsonProperty(Required = Required.Default)]
        public String ClientSecret { get; set; } = String.Empty;

        [JsonProperty("Authentication", ItemConverterType = typeof(StringEnumConverter))]
        public List<AuthenticationType> Authentication { get; set; }

        [JsonProperty(Required = Required.Always)]
        public String Username { get; set; } = String.Empty;

        [JsonProperty(Required = Required.Default)]
        public String Password { get; set; } = String.Empty;

        [JsonProperty(Required = Required.Default)]
        public List<SecurityClaim> Claims { get; set; }
    }
}
