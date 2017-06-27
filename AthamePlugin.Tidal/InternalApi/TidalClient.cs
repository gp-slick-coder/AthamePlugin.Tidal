using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using AthamePlugin.Tidal.InternalApi.Models;
using Newtonsoft.Json.Linq;

namespace AthamePlugin.Tidal.InternalApi
{
    public class TidalClient
    {
        private const string ApiRootUrl = "https://api.tidal.com/v1/";
        // Desktop token as at 31/05/2017
        private const string AppToken = "4zx46pyr9o8qZNRw";

        private const string HeaderTidalToken = "X-Tidal-Token";
        private const string HeaderTidalSession = "X-Tidal-SessionId";

        private readonly HttpClient httpClient = new HttpClient();
        private readonly List<KeyValuePair<string, string>> globalQuery = new List<KeyValuePair<string, string>>();
        private KeyValuePair<string, string> countryCodeParam;

        public int ItemsPerPage { get; set; }
        private TidalSession session;

        public TidalSession Session
        {
            get
            {
                return session;
            }
            set
            {
                session = value;
                UpdateClient();
            }
        }

        private void UpdateClient()
        {
            // Set country code
            if (countryCodeParam.Key != null) globalQuery.Remove(countryCodeParam);
            countryCodeParam = new KeyValuePair<string, string>("countryCode", session.CountryCode);
            globalQuery.Add(countryCodeParam);

            // Set session or token headers
            if (session.SessionId != null)
            {
                httpClient.DefaultRequestHeaders.Remove(HeaderTidalToken);
                httpClient.DefaultRequestHeaders.Add(HeaderTidalSession, session.SessionId);
            }
            else
            {
                httpClient.DefaultRequestHeaders.Remove(HeaderTidalSession);
                httpClient.DefaultRequestHeaders.Add(HeaderTidalToken, AppToken);
            }
        }

        private void Init()
        {
            ItemsPerPage = 100;
            httpClient.BaseAddress = new Uri(ApiRootUrl);
            UpdateClient();
        }

        public TidalClient()
        {
            session = new TidalSession {CountryCode = "WW"};
            Init();
        }

        public TidalClient(TidalSession savedSession)
        {
            
            session = savedSession;
            Init();
        }

        private string CreateQueryString(List<KeyValuePair<string, string>> requestQuery = null)
        {
            if (requestQuery == null)
            {
                requestQuery = globalQuery;
            }
            else
            {
                requestQuery.AddRange(globalQuery);
            }

            var queryString = String.Join("&", from keyValue in requestQuery select keyValue.Key + "=" + HttpUtility.UrlEncode(keyValue.Value));
            return queryString;
        }

        private string CreatePathWithQueryString(string path, List<KeyValuePair<string, string>> queryString = null)
        {
            var qs = CreateQueryString(queryString);
            return path + "?" + qs;
        }

        private T DeserializeOrThrow<T>(JObject result)
        {
            JToken statusToken;
            if (result.TryGetValue("status", out statusToken))
            {
                var asInt = statusToken.ToObject<int>();
                if (asInt != 200)
                {
                    throw result.ToObject<TidalException>();
                }
            }
            return result.ToObject<T>();
        }

        internal async Task<T> GetAsync<T>(string path, List<KeyValuePair<string, string>> queryString = null)
        {
            var result = JObject.Parse(await httpClient.GetStringAsync(CreatePathWithQueryString(path, queryString)));
            return DeserializeOrThrow<T>(result);
        }

        internal async Task<T> PostAsync<T>(string path, List<KeyValuePair<string, string>> queryString = null, List<KeyValuePair<string, string>> formParams = null)
        {
            if (formParams == null) formParams = new List<KeyValuePair<string, string>>();
            var response =
                await
                    httpClient.PostAsync(CreatePathWithQueryString(path, queryString),
                        new FormUrlEncodedContent(formParams));
            var result = JObject.Parse(await response.Content.ReadAsStringAsync());
            return DeserializeOrThrow<T>(result);
        }

        public async Task LoginWithUsernameAsync(string username, string password)
        {
            session = await PostAsync<TidalSession>("login/username", null, new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
                new KeyValuePair<string, string>("clientVersion", "2.2.1--7")
            });
            UpdateClient();
        }

        public async Task<TidalTrack> GetTrackAsync(int id)
        {
            return await GetAsync<TidalTrack>($"tracks/{id}");
        }

        public async Task<TidalAlbum> GetAlbumAsync(int id)
        {
            return await GetAsync<TidalAlbum>($"albums/{id}");
        }

        public PageManager<TidalTrack> GetAlbumItems(int id)
        {
            // Seems like Tidal is shifting to the albums/{id}/items endpoint...
            return new PageManager<TidalTrack>(this, $"albums/{id}/tracks", ItemsPerPage);
        }

        public async Task<TidalArtist> GetArtistAsync(int id)
        {
            return await GetAsync<TidalArtist>($"artists/{id}");
        }

        public PageManager<TidalTrack> GetArtistTopTracks(int id)
        {
            return new PageManager<TidalTrack>(this, $"artists/{id}/toptracks", ItemsPerPage);
        }

        public PageManager<TidalAlbum> GetArtistAlbums(int id)
        {
            return new PageManager<TidalAlbum>(this, $"artists/{id}/albums", ItemsPerPage);
        }

        public PageManager<TidalAlbum> GetArtistEpsAndSingles(int id)
        {
            var qsParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("filter", "EPSANDSINGLES")
            };
            return new PageManager<TidalAlbum>(this, $"artists/{id}/albums", ItemsPerPage, qsParams);
        }

        public async Task<TidalPlaylist> GetPlaylistAsync(string uuid)
        {
            return await GetAsync<TidalPlaylist>($"playlists/{uuid}");
        }

        public PlaylistPageManager GetPlaylistTracks(string uuid)
        {
            return new PlaylistPageManager(this, $"playlists/{uuid}/items", ItemsPerPage, new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("order", "INDEX"),
                new KeyValuePair<string, string>("orderDirection", "ASC")
            });
        }

        public async Task<TidalUser> GetUserAsync(int id)
        {
            return await GetAsync<TidalUser>($"users/{id}");
        }

        public async Task<TidalStream> GetStreamUrlAsync(int trackId, StreamingQuality quality)
        {
            
            return await GetAsync<TidalStream>($"tracks/{trackId}/streamUrl", new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("soundQuality", JToken.FromObject(quality).ToString())
            });
        }

        public async Task<TidalStream> GetOfflineUrlAsync(int trackId, StreamingQuality quality)
        {
            return await GetAsync<TidalStream>($"tracks/{trackId}/offlineUrl", new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("soundQuality", JToken.FromObject(quality).ToString())
            });
        }

        public async Task<UrlPostPaywallResponse> GetUrlPostPaywall(int trackId, StreamingQuality audioQuality,
            UrlUsageMode urlUsageMode)
        {
            return
                await
                    GetAsync<UrlPostPaywallResponse>($"tracks/{trackId}/urlpostpaywall",
                        new List<KeyValuePair<string, string>>()
                        {
                            new KeyValuePair<string, string>("audioquality", JToken.FromObject(audioQuality).ToString()),
                            new KeyValuePair<string, string>("urlusagemode", JToken.FromObject(urlUsageMode).ToString()),
                            new KeyValuePair<string, string>("assetpresentation", "FULL")
                        });
        }

    }
}
