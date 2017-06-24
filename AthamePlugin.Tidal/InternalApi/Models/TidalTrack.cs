using System;
using System.Collections.Generic;
using System.Linq;
using Athame.PluginAPI.Service;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AthamePlugin.Tidal.InternalApi.Models
{
    public class TidalTrack
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("duration")]
        [JsonConverter(typeof(SecondsToDurationConverter))]
        public TimeSpan? Duration { get; set; }

        [JsonProperty("replayGain")]
        public double ReplayGain { get; set; }

        [JsonProperty("peak")]
        public double Peak { get; set; }

        [JsonProperty("allowStreaming")]
        public bool AllowStreaming { get; set; }

        [JsonProperty("streamReady")]
        public bool StreamReady { get; set; }

        [JsonProperty("streamStartDate")]
        public DateTime StreamStartDate { get; set; }

        [JsonProperty("premiumStreamingOnly")]
        public bool PremiumStreamingOnly { get; set; }

        [JsonProperty("trackNumber")]
        public int TrackNumber { get; set; }

        [JsonProperty("volumeNumber")]
        public int VolumeNumber { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("popularity")]
        public int Popularity { get; set; }

        [JsonProperty("copyright")]
        public string Copyright { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("isrc")]
        public string Isrc { get; set; }

        [JsonProperty("editable")]
        public bool Editable { get; set; }

        [JsonProperty("explicit")]
        public bool Explicit { get; set; }

        [JsonProperty("audioQuality")]
        public StreamingQuality AudioQuality { get; set; }

        [JsonProperty("artist")]
        public FeaturedArtist Artist { get; set; }

        [JsonProperty("artists")]
        public List<FeaturedArtist> Artists { get; set; }

        [JsonProperty("album")]
        public TidalAlbum Album { get; set; }

        internal Track CreateAthameTrack()
        {
            // Always put main artists in the artist field
            var t = new Track
            {
                DiscNumber = VolumeNumber,
                TrackNumber = TrackNumber,
                Title = Title,
                Id = Id.ToString(),
                IsDownloadable = AllowStreaming,
                // Only use first artist name and picture for now
                Artist = NameHelpers.CreateMainArtist(Artists, Artist),
                CustomMetadata = new[]
                {
                    MetadataHelpers.ExplicitMetadata(Explicit),
                    MetadataHelpers.MasterMetadata(AudioQuality)
                }
            };



            // If the featured artists aren't already in the title, append them there
            if (!EnglishArtistNameJoiner.DoesTitleContainArtistString(this))
            {
                var nonMainArtists = (from artist in Artists
                                      where artist.Type != ArtistRole.Main
                                      select artist.Name).ToArray();
                if (nonMainArtists.Length > 0)
                {
                    t.Title += " " + EnglishArtistNameJoiner.JoinFeaturingArtists(nonMainArtists);
                }
            }
            t.Album = Album.CreateAthameAlbum();
            return t;
        }

    }
}
