using Newtonsoft.Json;
using System;
using TNDStudios.Web.ApiManager.Data.Soap;

namespace TNDStudios.Web.ApiManager.Data.Salesforce
{
    /// <summary>
    /// Default Salesforce object base class which notifications
    /// and other objects usually follow as a pattern
    /// </summary>
    [Serializable]
    [JsonObject]
    public class SalesforceObjectBase : SoapBase
    {
        /// <summary>
        /// The Xsi Type
        /// </summary>
        [JsonProperty(PropertyName = "@xsi:type")]
        public String ObjectType { get; set; }

        /// <summary>
        /// The namespace to define this Xml Schema
        /// </summary>
        [JsonProperty(PropertyName = "@xmlns:sf")]
        public String Namespace { get; set; }

        /// <summary>
        /// The object Id
        /// </summary>
        [JsonProperty(PropertyName = "sf:id")]
        public String Id { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SalesforceObjectBase() : base()
        {
        }
    }
}
