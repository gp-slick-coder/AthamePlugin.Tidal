using System;
using System.Collections.Generic;
using System.Linq;
using Athame.PluginAPI.Service;
using Newtonsoft.Json;

namespace AthamePlugin.Tidal.InternalApi.Models
{
    public class TidalAlbum
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("duration")]
        [JsonConverter(typeof(SecondsToDurationConverter))]
        public TimeSpan Duration { get; set; }

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
        public FeaturedArtist Artist { get; set; }

        [JsonProperty("artists")]
        public IList<FeaturedArtist> Artists { get; set; }

        internal List<TidalTrack> TidalTracks { get; set; }

        internal Album CreateAthameAlbum()
        { 
            var cmAlbum = new Album
            {
                Id = Id.ToString(),
                Title = Title,
                CoverPicture = new TidalPicture(Cover),
                Type = (AlbumType)TidalAlbumType
            };
            // On most calls the Album returned is a "lite" version, with only the properties above
            // available.
            if (Artist != null)
            {
                // Need only main artists
                cmAlbum.Artist = NameHelpers.CreateMainArtist(Artists, Artist);
            }
            if (TidalTracks != null)
            {
                cmAlbum.Tracks = (from track in TidalTracks select track.CreateAthameTrack()).ToList();
            }
            return cmAlbum;
        }

    }
}
