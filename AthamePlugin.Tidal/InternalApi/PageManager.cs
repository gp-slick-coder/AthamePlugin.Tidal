using System.Collections.Generic;
using System.Threading.Tasks;
using AthamePlugin.Tidal.InternalApi.Models;

namespace AthamePlugin.Tidal.InternalApi
{
    public class PageManager<T>
    {
        private readonly TidalClient client;
        private readonly string path;

        internal PageManager(TidalClient client, string path, int itemsPerPage)
        {
            ItemsPerPage = itemsPerPage;
            this.client = client;
            this.path = path;
        }

        private List<KeyValuePair<string, string>> CreateOffsetAndLimitParams()
        {
            var limit = 100;
            var offset = 0;

            limit = ItemsPerPage;
            if (LastPageRequested != null)
            {
                offset = LastPageRequested.Offset + limit;
            }

            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("limit", limit.ToString()),
                new KeyValuePair<string, string>("offset", offset.ToString())
            };
        }

        public List<T> AllItems { get; set; }

        public int ItemsPerPage { get; }

        public int TotalNumberOfItems { get; set; }

        public PaginatedList<T> LastPageRequested { get; set; }

        public async Task<PaginatedList<T>> LoadNextPageAsync()
        {
            var pageParams = CreateOffsetAndLimitParams();
            LastPageRequested = await client.GetAsync<PaginatedList<T>>(path, pageParams);
            AllItems.AddRange(LastPageRequested.Items);
            return LastPageRequested;
        }
    }
}
