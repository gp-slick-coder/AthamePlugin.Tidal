using System.Collections.Generic;
using System.Threading.Tasks;
using Athame.PluginAPI.Service;
using AthamePlugin.Tidal.InternalApi.Models;

namespace AthamePlugin.Tidal.InternalApi
{
    public class PageManager<T> : PagedMethod<T>
    {

        protected readonly TidalClient Client;
        protected readonly string Path;
        protected readonly List<KeyValuePair<string, string>> QueryString;
        protected readonly List<T> AllItemsBacking = new List<T>();

        internal PageManager(TidalClient client, string path, int itemsPerPage,
            List<KeyValuePair<string, string>> queryString = null)
            : base(itemsPerPage)
        {
            this.Client = client;
            this.Path = path;
            this.QueryString = queryString;
        }

        protected List<KeyValuePair<string, string>> CreateOffsetAndLimitParams()
        {
            var limit = ItemsPerPage;
            var offset = 0;

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

        public override async Task<IList<T>> GetNextPageAsync()
        {
            var pageParams = CreateOffsetAndLimitParams();
            if (QueryString != null) pageParams.AddRange(QueryString);
            LastPageRequested = await Client.GetAsync<PaginatedList<T>>(Path, pageParams);
            AllItemsBacking.AddRange(LastPageRequested.Items);
            return LastPageRequested.Items;
        }

        public override IList<T> AllItems => AllItemsBacking;

        public override int TotalNumberOfItems => LastPageRequested?.TotalNumberOfItems ?? -1;

        public override bool HasMoreItems
        {
            get
            {
                if (LastPageRequested == null) return true;
                var nextOffset = LastPageRequested.Offset + ItemsPerPage;
                return nextOffset <= TotalNumberOfItems;
            }
        }

        public PaginatedList<T> LastPageRequested { get; set; }
    }
}
