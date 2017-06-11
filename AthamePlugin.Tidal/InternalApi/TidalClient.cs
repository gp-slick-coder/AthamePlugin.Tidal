using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AthamePlugin.Tidal.InternalApi.Models;
using Newtonsoft.Json;
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

        public TidalSession Session { get; set; }

        private void UpdateClient()
        {
            // Set country code
            if (countryCodeParam.Key != null) globalQuery.Remove(countryCodeParam);
            countryCodeParam = new KeyValuePair<string, string>("countryCode", Session.CountryCode);
            globalQuery.Add(countryCodeParam);

            // Set session or token headers
            if (Session.SessionId != null)
            {
                httpClient.DefaultRequestHeaders.Remove(HeaderTidalToken);
                httpClient.DefaultRequestHeaders.Add(HeaderTidalSession, Session.SessionId);
            }
            else
            {
                httpClient.DefaultRequestHeaders.Remove(HeaderTidalSession);
                httpClient.DefaultRequestHeaders.Add(HeaderTidalToken, AppToken);
            }
        }

        private void Init()
        {
            httpClient.BaseAddress = new Uri(ApiRootUrl);
            UpdateClient();
        }

        public TidalClient()
        {
            Session = new TidalSession {CountryCode = "WW"};
            Init();
        }

        public TidalClient(TidalSession savedSession)
        {
            
            Session = savedSession;
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

        private List<KeyValuePair<string, string>> CreateOffsetAndLimitParams<T>(PageManager<T> pageManager = null)
        {
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("limit", pageManager?.Limit.ToString() ?? 100.ToString()),
                new KeyValuePair<string, string>("offset", pageManager?.NextOffset.ToString() ?? 0.ToString())
            };
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

        private async Task<T> GetAsync<T>(string path, List<KeyValuePair<string, string>> queryString = null)
        {
            var result = JObject.Parse(await httpClient.GetStringAsync(CreatePathWithQueryString(path, queryString)));
            return DeserializeOrThrow<T>(result);
        }

        private async Task<T> PostAsync<T>(string path, List<KeyValuePair<string, string>> queryString = null, List<KeyValuePair<string, string>> formParams = null)
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
            Session = await PostAsync<TidalSession>("login/username", null, new List<KeyValuePair<string, string>>
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

        public async Task<PaginatedList<TidalTrack>> GetAlbumItemsAsync(int id, PageManager<TidalTrack> pageManager = null)
        {
            return await GetAsync<PaginatedList<TidalTrack>>($"albums/{id}/items", CreateOffsetAndLimitParams(pageManager));
        }

        public async Task<TidalArtist> GetArtistAsync(int id)
        {
            return await GetAsync<TidalArtist>($"artists/{id}");
        }

        public async Task<PaginatedList<TidalTrack>> GetArtistTopTracksAsync(int id, PageManager<TidalTrack> pageManager = null)
        {
            return
                await GetAsync<PaginatedList<TidalTrack>>($"artists/{id}/toptracks", CreateOffsetAndLimitParams(pageManager));
        }

        public async Task<PaginatedList<TidalAlbum>> GetArtistAlbumsAsync(int id, PageManager<TidalAlbum> pageManager = null)
        {
            return await GetAsync<PaginatedList<TidalAlbum>>($"artists/{id}/albums", CreateOffsetAndLimitParams(pageManager));
        }

        public async Task<PaginatedList<TidalAlbum>> GetArtistEpsAndSinglesAsync(int id,
            PageManager<TidalAlbum> pageManager = null)
        {
            var qsParams = CreateOffsetAndLimitParams(pageManager);
            qsParams.Add(new KeyValuePair<string, string>("filter", "EPSANDSINGLES"));
            return await GetAsync<PaginatedList<TidalAlbum>>($"artists/{id}/albums", qsParams);
        }

        public async Task<TidalPlaylist> GetPlaylistAsync(string uuid)
        {
            return await GetAsync<TidalPlaylist>($"playlists/{uuid}");
        }


    }
}
