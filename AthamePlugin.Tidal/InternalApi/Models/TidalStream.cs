using Newtonsoft.Json;

namespace AthamePlugin.Tidal.InternalApi.Models
{
    public class TidalStream
    {

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("trackId")]
        public int TrackId { get; set; }

        [JsonProperty("playTimeLeftInMinutes")]
        public int PlayTimeLeftInMinutes { get; set; }

        [JsonProperty("soundQuality")]
        public StreamingQuality SoundQuality { get; set; }

        [JsonProperty("encryptionKey")]
        public string EncryptionKey { get; set; }

        [JsonProperty("codec")]
        public string Codec { get; set; }
    }


}
