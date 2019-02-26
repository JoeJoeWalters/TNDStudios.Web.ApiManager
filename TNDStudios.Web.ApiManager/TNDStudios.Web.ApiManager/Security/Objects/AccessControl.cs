using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TNDStudios.Web.ApiManager.Security.Objects
{
    [JsonObject]
    public class AccessControl
    {
        [JsonProperty]
        public List<SecurityUser> Users { get; set; }
    }
}
