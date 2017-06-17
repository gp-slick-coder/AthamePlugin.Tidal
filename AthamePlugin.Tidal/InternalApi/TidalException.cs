using System;
using Newtonsoft.Json;

namespace AthamePlugin.Tidal.InternalApi
{
    public class TidalException : Exception
    {
        [JsonProperty("status")]
        public int Status { get; set; }
        [JsonProperty("subStatus")]
        public int SubStatus { get; set; }

        [JsonProperty("userMessage")]
        public string UserMessage { get; set; }

        public override string Message => $"{Status}/{SubStatus}: {UserMessage}";
    }
}
