using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AthamePlugin.Tidal.InternalApi.Models
{
    public class TidalArtist
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("picture")]
        public string PictureId { get; set; }

        [JsonProperty("popularity")]
        public int Popularity { get; set; }
    }
}
