using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AthamePlugin.Tidal.InternalApi.Models;

namespace AthamePlugin.Tidal.InternalApi.PagedCalls
{
    public class AlbumItemsPageManager : PageManager<TidalAlbum>
    {
        public AlbumItemsPageManager(int itemsPerPage) : base(itemsPerPage)
        {
        }

        public override Task<PaginatedList<TidalAlbum>> LoadNextPageAsync()
        {
            throw new NotImplementedException();
        }
    }
}
