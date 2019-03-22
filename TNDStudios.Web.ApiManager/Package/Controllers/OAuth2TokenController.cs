using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TNDStudios.Web.ApiManager.Controllers
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

    /// <summary>
    /// On successful verification of a token request
    /// https://www.oauth.com/oauth2-servers/access-tokens/access-token-response/
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class OAuthTokenSuccess
    {
        [JsonProperty(Required = Required.Always, PropertyName = "access_token")]
        public String AccessToken { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "token_type")]
        public String TokenType { get; set; } = "bearer";

        [JsonProperty(Required = Required.AllowNull, PropertyName = "expires_in")]
        public Int16 ExpiresIn { get; set; } = (Int16)3600;

        [JsonProperty(Required = Required.AllowNull, PropertyName = "refresh_token")]
        public String RefreshToken { get; set; } = null;

        [JsonProperty(Required = Required.AllowNull, PropertyName = "scope")]
        public String Scope { get; set; } = null;
    }

    /// <summary>
    /// On unsuccessful verification of a token request
    /// https://www.oauth.com/oauth2-servers/access-tokens/access-token-response/
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class OAuthTokenFailure
    {
        /// <summary>
        /// List of standard error reason types
        /// </summary>
        public enum ReasonType
        {
            invalid_request,
            invalid_client,
            invalid_grant,
            invalid_scope,
            unauthorized_client,
            unsupported_grant_type
        }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(Required = Required.Always, PropertyName = "error")]
        public ReasonType Reason { get; set; }

        [JsonProperty(Required = Required.AllowNull, PropertyName = "error_description")]
        public String ReasonDescription { get; set; } = null;

        [JsonProperty(Required = Required.AllowNull, PropertyName = "error_uri")]
        public String ReasonUri { get; set; } = null;
    }

    public class OAuth2TokenController : ControllerBase
    {
        public ILogger Logger { get; set; }

        public OAuth2TokenController(ILogger logger)
            => Logger = logger;

        [Consumes(@"application/json")]
        [Produces(@"application/json")]
        [ResponseCache(NoStore = true)]
        [HttpPost]
        public virtual ActionResult Post(OAuthTokenRequest request)
        {
            if ((new Random()).NextDouble() > (Double)0.5)
                return new OkObjectResult(
                    new OAuthTokenSuccess()
                    {
                        AccessToken = Guid.NewGuid().ToString(),
                        ExpiresIn = 3600,
                        RefreshToken = Guid.NewGuid().ToString(),
                        Scope = "test",
                        TokenType = "bearer"
                    });
            else
                return new BadRequestObjectResult(
                    new OAuthTokenFailure()
                    {
                        Reason = OAuthTokenFailure.ReasonType.unauthorized_client,
                        ReasonDescription = "Reason for the failure here",
                        ReasonUri = "url of the failure code"
                    });
        }
    }
}
