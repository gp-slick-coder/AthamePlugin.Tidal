using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AthamePlugin.Tidal.InternalApi
{
    public class TidalSession
    {
        [JsonProperty("userId")]
        public int UserId { get; set; }
        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }
        [JsonProperty("sessionId")]
        public string SessionId { get; set; }
    }
}
