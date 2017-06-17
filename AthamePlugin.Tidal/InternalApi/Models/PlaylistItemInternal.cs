using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AthamePlugin.Tidal.InternalApi.Models
{
    internal class PlaylistItemInternal
    {
        [JsonProperty("item")]
        public JObject Item { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
