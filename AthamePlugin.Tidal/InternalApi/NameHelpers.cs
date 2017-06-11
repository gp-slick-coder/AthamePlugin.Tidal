using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Athame.PluginAPI.Service;
using AthamePlugin.Tidal.InternalApi.Models;

namespace AthamePlugin.Tidal.InternalApi
{
    internal static class NameHelpers
    {
        internal static Artist CreateMainArtist(IList<FeaturedArtist> artists, FeaturedArtist defaultArtist)
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
            
            if (!String.IsNullOrEmpty(tidalTrack.Version))
            {
                if (settings.AppendVersionToTrackTitle)
                {
                    if (settings.DontAppendAlbumVersion)
                    {
                        if (!tidalTrack.Version.Contains(albumVersion))
                        {
                            t.Title += " (" + tidalTrack.Version + ")";
                        }
                    }
                    else
                    {
                        t.Title += " (" + tidalTrack.Version + ")";
                    }
                }
            }
        }
    }
}
