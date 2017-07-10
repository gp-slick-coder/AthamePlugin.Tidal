using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Athame.PluginAPI;
using Athame.PluginAPI.Downloader;
using Athame.PluginAPI.Service;
using AthamePlugin.Tidal.InternalApi;
using AthamePlugin.Tidal.InternalApi.Decryption;
using AthamePlugin.Tidal.InternalApi.Models;

namespace AthamePlugin.Tidal
{
    public class TidalService : MusicService, IUsernamePasswordAuthenticationAsync
    {
        public override int ApiVersion => 2;

        public override PluginInfo Info => new PluginInfo
        {
            Author = "svbnet",
            Description = "Plugin for Tidal music service.",
            Name = "Tidal",
            Website = new Uri("https://github.com/svbnet/AthamePlugin.Tidal")
        };

        private TidalClient client;
        private TidalServiceSettings settings = new TidalServiceSettings();
        private const string TidalWebDomain = "listen.tidal.com";

        public TidalService()
        {
            client = new TidalClient();
        }

        private AccountInfo AccountInfoFromUser(TidalUser user)
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

        public override IDownloader GetDownloader(TrackFile t)
        {
            var tidalTrackFile = (TidalTrackFile)t;
            if (tidalTrackFile.FileKey == null)
            {
                return new HttpDownloader();
            }
            return new TidalEncryptedDownloader();
        }

        public override async Task<TrackFile> GetDownloadableTrackAsync(Track track)
        {
            var response =
                await
                    client.GetUrlPostPaywall(Int32.Parse(track.Id), settings.StreamQuality,
                        settings.UseOfflineUrl ? UrlUsageMode.Offline : UrlUsageMode.Stream);
            var result = new TidalTrackFile
            {
                DownloadUri = new Uri(response.Urls.First()),
                Track = track
            };
            // We can assume the MIME type and bitrate from the **returned** sound quality
            // It is unwise to use the stream quality stored in settings as users with lossless
            // subscriptions will get lossy streams simply because lossless streams are unavailable
            switch (response.AudioQuality)
            {
                case StreamingQuality.Low:
                    result.FileType = MediaFileTypes.Mpeg4Audio;
                    result.BitRate = 96 * 1000;
                    break;
                case StreamingQuality.High:
                    result.FileType = MediaFileTypes.Mpeg4Audio;
                    result.BitRate = 320 * 1000;
                    break;
                case StreamingQuality.HiRes:
                case StreamingQuality.Lossless:
                    result.FileType = MediaFileTypes.FreeLosslessAudioCodec;
                    // Bitrate doesn't really matter since it's lossless
                    result.BitRate = -1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (response.SecurityToken != null)
            {
                result.FileKey = TidalDecryptor.ParseFileKey(Convert.FromBase64String(response.SecurityToken));
            }
            return result;
        }

        public override async Task<Playlist> GetPlaylistAsync(string playlistId)
        {
            var playlist = await client.GetPlaylistAsync(playlistId);
            var itemsPages = client.GetPlaylistTracks(playlistId);
            await itemsPages.LoadAllPagesAsync();

            return new Playlist
            {
                Title = playlist.Title,
                PlaylistPicture = new PlaylistPicture(playlist.Image),
                Tracks = (from t in itemsPages.AllItems select t.CreateAthameTrack(settings)).ToList()
            };
        }

        public override UrlParseResult ParseUrl(Uri url)
        {
            var pathParts = url.LocalPath.Split('/');
            if (pathParts.Length <= 2) return null;
            var ctype = pathParts[1];
            var id = pathParts[2];
            var result = new UrlParseResult { Id = id, Type = MediaType.Unknown, OriginalUri = url };
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
            var tidalAlbum = await client.GetAlbumAsync(Int32.Parse(albumId));
            if (!withTracks)
            {
                return tidalAlbum.CreateAthameAlbum(settings);
            }
            var tidalTracksPage = client.GetAlbumItems(Int32.Parse(albumId));
            await tidalTracksPage.LoadAllPagesAsync();
            var cmAlbum = tidalAlbum.CreateAthameAlbum(settings);
            var cmTracks = new List<Track>();
            foreach (var track in tidalTracksPage.AllItems)
            {
                var cmTrack = track.CreateAthameTrack(settings);
                cmTrack.Album = cmAlbum;
                cmTrack.Year = tidalAlbum.ReleaseDate.Year;
                cmTracks.Add(cmTrack);
            }
            cmAlbum.Tracks = cmTracks;
            return cmAlbum;
        }

        public override async Task<Track> GetTrackAsync(string trackId)
        {
            var track = await client.GetTrackAsync(Int32.Parse(trackId));
            var album = await client.GetAlbumAsync(track.Album.Id);
            var athameTrack = track.CreateAthameTrack(settings);
            athameTrack.Album = album.CreateAthameAlbum(settings);
            return athameTrack;
        }

        public void Reset()
        {
            client = new TidalClient();
            Account = null;
            settings.User = null;
            settings.Session = null;
        }

        public AccountInfo Account { get; private set; }
        public bool IsAuthenticated => client.Session.SessionId != null;

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
                if (settings?.User != null)
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




        public override void Init(AthameApplication application, PluginContext pluginContext)
        {

        }

        public bool HasSavedSession => settings.Session?.SessionId != null && settings.User != null;

        public Task<bool> RestoreAsync()
        {
            if (settings.Session == null) return Task.FromResult(false);
            try
            {

                client.Session = settings.Session;
                Account = AccountInfoFromUser(settings.User);
            }
            catch (TidalException)
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(client.Session != null);
        }

        public async Task<bool> AuthenticateAsync(string username, string password, bool rememberUser)
        {
            try
            {
                await client.LoginWithUsernameAsync(username, password);
            }
            catch (TidalException)
            {
                return false;
            }
            var user = await client.GetUserAsync(client.Session.UserId);
            Account = AccountInfoFromUser(user);
            if (rememberUser)
            {
                settings.User = user;
                settings.Session = client.Session;
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
