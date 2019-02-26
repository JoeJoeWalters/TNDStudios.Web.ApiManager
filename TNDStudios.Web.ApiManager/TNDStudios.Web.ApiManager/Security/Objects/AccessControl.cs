using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TNDStudios.Web.ApiManager.Security.Objects
{
    /// <summary>
    /// Access control list that should contain the users that are validated against
    /// not always stored in a database as the users may contain keys that are generated 
    /// only whilst the session is alive etc.
    /// </summary>
    [JsonObject]
    public class AccessControl
    {
        [JsonProperty]
        public List<SecurityUser> Users { get; set; }
    }
}
