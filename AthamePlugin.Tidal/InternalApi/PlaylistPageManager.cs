using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AthamePlugin.Tidal.InternalApi.Models;

namespace AthamePlugin.Tidal.InternalApi
{
    public class PlaylistPageManager : PageManager<TidalTrack>
    {
        internal PlaylistPageManager(TidalClient client, string path, int itemsPerPage, List<KeyValuePair<string, string>> queryString = null) 
            : base(client, path, itemsPerPage, queryString)
        {

        }

        public override async Task<IList<TidalTrack>> GetNextPageAsync()
        {
            var pageParams = CreateOffsetAndLimitParams();
            if (QueryString != null) pageParams.AddRange(QueryString);

            // This is where we deviate. Since Tidal playlists can contain videos and tracks (and we have no use for videos yet),
            // we use PlaylistItemInternal to represent the playlist item as a JObject (since we don't want to have to create a 
            // whole new set of types just for videos), then if it's a track we can deserialise it as a TidalTrack.
            var returnedPage = await Client.GetAsync<PaginatedList<PageListItem>>(Path, pageParams);

            // Fool the caller into thinking we only get tracks from the server (heh... heh...)
            LastPageRequested = new PaginatedList<TidalTrack>
            {
                Limit = returnedPage.Limit,
                Offset = returnedPage.Offset,
                TotalNumberOfItems = returnedPage.TotalNumberOfItems,
                Items = (from playlistItem in returnedPage.Items
                    where playlistItem.Type == "track"
                    select playlistItem.Item.ToObject<TidalTrack>()).ToList(),
            };

            AllItemsBacking.AddRange(LastPageRequested.Items);
            return LastPageRequested.Items;
        }
    }
}
