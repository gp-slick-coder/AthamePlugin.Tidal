using System;
using System.Threading.Tasks;
using AthamePlugin.Tidal.InternalApi.Models;

namespace AthamePlugin.Tidal.InternalApi.PagedCalls
{
    public class AlbumItemsPageManager : PageManager<TidalAlbum>
    {
        private readonly TidalClient client;

        internal AlbumItemsPageManager(TidalClient client, int itemsPerPage) : base(itemsPerPage)
        {
            this.client = client;
        }

        public override Task<PaginatedList<TidalAlbum>> LoadNextPageAsync()
        {
            
        }
    }
}
