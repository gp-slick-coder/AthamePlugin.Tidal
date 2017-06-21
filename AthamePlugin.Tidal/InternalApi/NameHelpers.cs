using System;
using System.Collections.Generic;
using System.Linq;
using Athame.PluginAPI.Service;
using AthamePlugin.Tidal.InternalApi.Models;

namespace AthamePlugin.Tidal.InternalApi
{
    internal static class NameHelpers
    {
        private const string AlbumVersion = "Album Version";

        internal static Artist CreateMainArtist(List<FeaturedArtist> artists, FeaturedArtist defaultArtist)
        {
            return new Artist
            {
                Id = defaultArtist.Id.ToString(),
                Name = EnglishArtistNameJoiner.JoinArtistNames((from artist in artists
                                                                where artist.Type == ArtistRole.Main
                                                                select artist.Name).ToArray())
            };
        }

        internal static string CreateTrackTitle(TidalServiceSettings settings, TidalTrack tidalTrack)
        {
            if (String.IsNullOrEmpty(tidalTrack.Version)) return tidalTrack.Title;
            if (!settings.AppendVersionToTrackTitle) return tidalTrack.Title;
            if (settings.DontAppendAlbumVersion)
            {
                if (tidalTrack.Version.IndexOf(AlbumVersion, StringComparison.OrdinalIgnoreCase) == -1)
                {
                    return tidalTrack.Title + " (" + tidalTrack.Version + ")";
                }
            }
            else
            {
                return tidalTrack.Title + " (" + tidalTrack.Version + ")";
            }
            return tidalTrack.Title;
        }
    }
}
