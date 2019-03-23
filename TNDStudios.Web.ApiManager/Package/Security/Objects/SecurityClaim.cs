using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TNDStudios.Web.ApiManager.Security.Objects
{
    /// <summary>
    /// The "claim" of what a security check is needed or what a 
    /// user can claim they can do.
    /// </summary>
    [JsonObject]
    public class SecurityClaim
    {
        /// <summary>
        /// Constants used for applying property values to the encoded claims
        /// </summary>
        public const String ClaimPrefixIdentifier = "sec";
        public const String ClaimSeperator = "_";
        public const String TypePropertyName = "type";
        public const String CategoryPropertyName = "category";
        public const String PermissionPropertyName = "permission";

        [JsonProperty(Required = Required.Always)]
        public String Name { get; set; }

        [JsonProperty(Required = Required.Default)]
        public List<String> Categories { get; set; }

        [JsonProperty(Required = Required.Default)]
        public List<String> Permissions { get; set; }
    }
}
