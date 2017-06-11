using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Athame.PluginAPI.Service;
using Newtonsoft.Json;

namespace AthamePlugin.Tidal.InternalApi.Models
{
    public class TidalAlbum : Album
    {

        [JsonProperty("id")]
        public new int Id { get; set; }

        [JsonProperty("title")]
        public new string Title { get; set; }

        [JsonProperty("duration")]
        [JsonConverter(typeof(SecondsToDurationConverter))]
        public new TimeSpan Duration { get; set; }

        [JsonProperty("streamReady")]
        public bool StreamReady { get; set; }

        [JsonProperty("streamStartDate")]
        public DateTime StreamStartDate { get; set; }

        [JsonProperty("allowStreaming")]
        public bool AllowStreaming { get; set; }

        [JsonProperty("premiumStreamingOnly")]
        public bool PremiumStreamingOnly { get; set; }

        [JsonProperty("numberOfTracks")]
        public int NumberOfTracks { get; set; }

        [JsonProperty("numberOfVideos")]
        public int NumberOfVideos { get; set; }

        [JsonProperty("numberOfVolumes")]
        public int NumberOfVolumes { get; set; }

        [JsonProperty("releaseDate")]
        public DateTime ReleaseDate { get; set; }

        [JsonProperty("copyright")]
        public string Copyright { get; set; }

        [JsonProperty("type")]
        public TidalAlbumType TidalAlbumType { get; set; }

        public new AlbumType Type
        {
            get
            {
                switch (TidalAlbumType)
                {
                    case TidalAlbumType.Album:
                        return AlbumType.Album;
                    case TidalAlbumType.Ep:
                        return AlbumType.EP;
                    case TidalAlbumType.Single:
                        return AlbumType.Single;
                }
                return AlbumType.Album;
            }
        }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("cover")]
        public string Cover { get; set; }

        [JsonProperty("videoCover")]
        public string VideoCover { get; set; }

        [JsonProperty("explicit")]
        public bool Explicit { get; set; }

        [JsonProperty("upc")]
        public string Upc { get; set; }

        [JsonProperty("popularity")]
        public int Popularity { get; set; }

        [JsonProperty("audioQuality")]
        public string AudioQuality { get; set; }

        [JsonProperty("artist")]
        public FeaturedArtist TidalArtist { get; set; }

        private Artist artist;

        public new Artist Artist => artist ?? (artist = NameHelpers.CreateMainArtist(Artists, TidalArtist));

        [JsonProperty("artists")]
        public IList<FeaturedArtist> Artists { get; set; }

        internal List<TidalTrack> TidalTracks { get; set; }

        public new List<Track> Tracks { get; set; }

    }
}
