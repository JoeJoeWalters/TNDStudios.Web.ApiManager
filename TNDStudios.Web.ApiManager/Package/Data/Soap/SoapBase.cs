using Newtonsoft.Json;
using System;

namespace TNDStudios.Web.ApiManager.Data.Soap
{
    /// <summary>
    /// Base class to handle elements of the soap message
    /// and store any unmapped items in the property bag so that
    /// nothing is lost
    /// </summary>
    [Serializable]
    [JsonObject]
    public class SoapBase : JsonBase
    {
        /// <summary>
        /// Default Constructor to set up any undefined elements
        /// </summary>
        public SoapBase() : base()
        {
        }
    }
}
