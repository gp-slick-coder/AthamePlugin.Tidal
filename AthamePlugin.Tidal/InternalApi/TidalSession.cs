using Newtonsoft.Json;

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
