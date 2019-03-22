using Newtonsoft.Json;
using System;

namespace TNDStudios.Web.ApiManager.Security.OAuth
{
    /// <summary>
    /// Request for a set of credentials (client id, secret etc.)
    /// to be validated to give back an access token
    /// https://www.oauth.com/oauth2-servers/access-tokens/client-credentials/
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class OAuthTokenRequest
    {
        [JsonProperty(Required = Required.Always, PropertyName = "grant_type")]
        public String GrantType { get; set; } = "client_credentials";

        [JsonProperty(Required = Required.Always, PropertyName = "client_id")]
        public String ClientId { get; set; } = null;

        [JsonProperty(Required = Required.Always, PropertyName = "client_secret")]
        public String ClientSecret { get; set; } = null;
    }
}
