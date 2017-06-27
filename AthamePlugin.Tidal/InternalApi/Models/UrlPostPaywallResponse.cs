using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AthamePlugin.Tidal.InternalApi.Models
{
    public class UrlPostPaywallResponse
    {

        [JsonProperty("urls")]
        public IList<string> Urls { get; set; }

        [JsonProperty("trackId")]
        public int TrackId { get; set; }

        [JsonProperty("assetPresentation")]
        public string AssetPresentation { get; set; }

        [JsonProperty("audioQuality")]
        public StreamingQuality AudioQuality { get; set; }

        [JsonProperty("codec")]
        public TidalCodec Codec { get; set; }

        [JsonProperty("securityType")]
        public string SecurityType { get; set; }

        [JsonProperty("securityToken")]
        public string SecurityToken { get; set; }
    }
}
