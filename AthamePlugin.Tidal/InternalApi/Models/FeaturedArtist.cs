using Newtonsoft.Json;

namespace AthamePlugin.Tidal.InternalApi.Models
{
    public class FeaturedArtist
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("type")]
        public ArtistRole Type { get; set; }
    }
}
