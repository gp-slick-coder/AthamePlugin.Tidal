using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AthamePlugin.Tidal.InternalApi.Models
{
    public class TidalPlaylist
    {

        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("numberOfTracks")]
        public int NumberOfTracks { get; set; }

        [JsonProperty("numberOfVideos")]
        public int NumberOfVideos { get; set; }

        [JsonProperty("creator")]
        public TidalArtist Creator { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("duration")]
        public int Duration { get; set; }

        [JsonProperty("lastUpdated")]
        public DateTime LastUpdated { get; set; }

        [JsonProperty("created")]
        public DateTime Created { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("publicPlaylist")]
        public bool PublicPlaylist { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("popularity")]
        public int Popularity { get; set; }
    }
}
