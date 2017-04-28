using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Athame.PluginAPI;
using Athame.PluginAPI.Downloader;
using Athame.PluginAPI.Service;
using OpenTidl;
using OpenTidl.Enums;
using OpenTidl.Methods;
using OpenTidl.Models;
using OpenTidl.Transport;

namespace AthamePlugin.Tidal
{
    public class TidalService : MusicService, IUsernamePasswordAuthenticationAsync
    {
        private readonly OpenTidlClient client;
        private OpenTidlSession session;
        private TidalServiceSettings settings = new TidalServiceSettings();
        private const string TidalWebDomain = "listen.tidal.com";

        public TidalService()
        {
            client = new OpenTidlClient(ClientConfiguration.Default);
        }

        private Track CreateTrack(TrackModel tidalTrack)
        {
            const string albumVersion = "Album Version";
            // Always put main artists in the artist field
            var t = new Track
            {
                DiscNumber = tidalTrack.VolumeNumber,
                TrackNumber = tidalTrack.TrackNumber,
                Title = tidalTrack.Title,
                Id = tidalTrack.Id.ToString(),
                IsDownloadable = tidalTrack.AllowStreaming

            };
            // Only use first artist name and picture for now
            t.Artist = CreateArtist(tidalTrack.Artists, tidalTrack.Artist);
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
            // If the featured artists aren't already in the title, append them there
            if (!EnglishArtistNameJoiner.DoesTitleContainArtistString(tidalTrack))
            {
                var nonMainArtists = (from artist in tidalTrack.Artists
                    where artist.Type != EnglishArtistNameJoiner.ArtistMain
                    select artist.Name).ToArray();
                if (nonMainArtists.Length > 0)
                {
                    t.Title += " " + EnglishArtistNameJoiner.JoinFeaturingArtists(nonMainArtists);
                }
            }
            t.Album = CreateAlbum(tidalTrack.Album);
            return t;
        }

        private Track CreateTrack(AlbumModel tidalAlbum, TrackModel tidalTrack)
        {
            var t = CreateTrack(tidalTrack);
            if (tidalAlbum.ReleaseDate != null) t.Year = tidalAlbum.ReleaseDate.Value.Year;
            t.Album = CreateAlbum(tidalAlbum);
            return t;
        }

        private Album CreateAlbum(AlbumModel album, List<TrackModel> tracks)
        {
            var a = CreateAlbum(album);
            a.Tracks = new List<Track>(from t in tracks select CreateTrack(album, t));
            return a;
        }

        private const int AlbumArtSize = 1280;
        private const string AlbumArtUrlFormat = "https://resources.tidal.com/images/{0}/{1}x{1}.jpg";

        private Artist CreateArtist(ArtistModel[] artists, ArtistModel defaultArtist)
        {
            return new Artist
            {
                Id = defaultArtist.Id.ToString(),
                Name = EnglishArtistNameJoiner.JoinArtistNames((from artist in artists
                                                                where artist.Type == EnglishArtistNameJoiner.ArtistMain
                                                                select artist.Name).ToArray())
            };
        }

        private Album CreateAlbum(AlbumModel album)
        {
            var coverUrl = String.Format(AlbumArtUrlFormat, album.Cover.Replace('-', '/'), AlbumArtSize);
            var cmAlbum = new Album
            {
                Id = album.Id.ToString(),
                Title = album.Title,
                CoverUri = new Uri(coverUrl)
            };
            // On most calls the Album returned is a "lite" version, with only the properties above
            // available.
            if (album.Artist != null)
            {
                // Need only main artists
                cmAlbum.Artist = CreateArtist(album.Artists, album.Artist);
            }
            return cmAlbum;
        }

        private AccountInfo AccountInfoFromUser(UserModel user)
        {
            return new AccountInfo
            {
                DisplayName =
                    String.IsNullOrEmpty(user.FirstName) && String.IsNullOrEmpty(user.LastName)
                        ? null
                        : String.Concat(user.FirstName, " ", user.LastName),
                DisplayId = user.Email
            };
        }

        public override async Task<TrackFile> GetDownloadableTrackAsync(Track track)
        {
            var response = await session.GetTrackOfflineUrl(Int32.Parse(track.Id), settings.StreamQuality);
            var result = new TrackFile {DownloadUri = new Uri(response.Url), Track = track};
            // We can assume the MIME type and bitrate from the **returned** sound quality
            // It is unwise to use the stream quality stored in settings as users with lossless
            // subscriptions will get lossy streams simply because lossless streams are unavailable
            switch (response.SoundQuality)
            {
                case SoundQuality.LOW:
                    result.FileType = MediaFileTypes.Mpeg4Audio;
                    result.BitRate = 96 * 1000;
                    break;
                case SoundQuality.HIGH:
                    result.FileType = MediaFileTypes.Mpeg4Audio;
                    result.BitRate = 320 * 1000;
                    break;
                case SoundQuality.LOSSLESS:
                    result.FileType = MediaFileTypes.FreeLosslessAudioCodec;
                    // Bitrate doesn't really matter since it's lossless
                    result.BitRate = -1;
                    break;
                case SoundQuality.LOSSLESS_HD:
                    // This seems to be obsolete so I'll just wait and see
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return result;
        }

        public override async Task<Playlist> GetPlaylistAsync(string playlistId)
        {
            var playlist = await session.GetPlaylist(playlistId);
            var tracks = await session.GetPlaylistTracks(playlistId);
            return new Playlist
            {
                Title = playlist.Title,
                Tracks = (from t in tracks.Items select CreateTrack(t)).ToList()
            };
        }

        public override UrlParseResult ParseUrl(Uri url)
        {
            if (url.Host != TidalWebDomain)
            {
                return null;
            }
            var pathParts = url.LocalPath.Split('/');
            if (pathParts.Length <= 2) return null;
            var ctype = pathParts[1];
            var id = pathParts[2];
            var result = new UrlParseResult {Id = id, Type = MediaType.Unknown, OriginalUri = url};
            switch (ctype)
            {
                case "album":
                    result.Type = MediaType.Album;
                    break;

                case "track":
                    result.Type = MediaType.Track;
                    break;

                case "artist":
                    result.Type = MediaType.Artist;
                    break;

                case "playlist":
                    result.Type = MediaType.Playlist;
                    break;

                default:
                    result.Type = MediaType.Unknown;
                    break;
            }
            return result;
        }

        public override Task<SearchResult> SearchAsync(string searchText, MediaType typesToRetrieve)
        {
            
            throw new NotImplementedException();
        }

        public override async Task<Album> GetAlbumAsync(string albumId, bool withTracks)
        {
            var tidalAlbum = await client.GetAlbum(Int32.Parse(albumId));
            if (!withTracks)
            {
                return CreateAlbum(tidalAlbum);
            }
            var tidalTracks = await client.GetAlbumTracks(Int32.Parse(albumId));
            var cmAlbum = CreateAlbum(tidalAlbum);
            var cmTracks = new List<Track>();
            foreach (var track in tidalTracks.Items)
            {
                var cmTrack = CreateTrack(track);
                cmTrack.Album = cmAlbum;
                if (tidalAlbum.ReleaseDate != null) cmTrack.Year = tidalAlbum.ReleaseDate.Value.Year;
                cmTracks.Add(cmTrack);
            }
            cmAlbum.Tracks = cmTracks;
            return cmAlbum;
        }

        public override async Task<Track> GetTrackAsync(string trackId)
        {
            var track = await client.GetTrack(Int32.Parse(trackId));
            var album = await client.GetAlbum(track.Album.Id);
            return CreateTrack(album, track);
        }

        public void Reset()
        {
            Account = null;
            settings.User = null;
            session = null;
        }

        public AccountInfo Account { get; private set; }
        public bool IsAuthenticated => session != null;

        public override Control GetSettingsControl()
        {
            return new TidalSettingsControl(settings);
        }

        public override object Settings
        {
            get { return settings; }
            set
            {
                settings = (TidalServiceSettings)value ?? new TidalServiceSettings();
                if (settings != null)
                {
                    Account = AccountInfoFromUser(settings.User);
                }
            }
        }

        public override Uri[] BaseUri
            => new[]
            {
                new Uri("http://" + TidalWebDomain), new Uri("https://" + TidalWebDomain),
                new Uri("http://tidal.com"), new Uri("https://tidal.com"),
            };

        public override string Name => "Tidal";
        public override string Description => "Plugin for Tidal music service.";
        public override string Author => "svbnet";
        public override Uri Website => new Uri("https://svbnet.co");
        public override PluginVersion ApiVersion => PluginVersion.V1;

        public override void Init(AthameApplication application, PluginContext pluginContext)
        {
            
        }

        public bool HasSavedSession => settings.SessionToken != null;

        public async Task<bool> RestoreAsync()
        {
            if (settings.SessionToken == null) return false;
            try
            {
                session = await client.RestoreSession(settings.SessionToken);
                Account = AccountInfoFromUser(settings.User);
            }
            catch (OpenTidlException)
            {
                return false;
            }
            return session != null;
        }

        public async Task<bool> AuthenticateAsync(string username, string password, bool rememberUser)
        {
            try
            {
                session = await client.LoginWithUsername(username, password);
            }
            catch (OpenTidlException)
            {
                return false;
            }
            var user = await session.GetUser();
            Account = AccountInfoFromUser(user);
            if (rememberUser)
            {
                settings.SessionToken = session.SessionId;
            }
            return true;
        }

        public string SignInHelpText => "Enter your Tidal username and password:";

        public IReadOnlyCollection<SignInLink> SignInLinks => new[]
        {
            new SignInLink {DisplayName = "Forgot password?", Link = new Uri("https://listen.tidal.com/")}
        };
    }
}
