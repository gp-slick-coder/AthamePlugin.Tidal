using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AthamePlugin.Tidal.InternalApi.Models;

namespace AthamePlugin.Tidal.InternalApi
{
    public class GenericPageManager : PageManager<PageListItem>
    {
        public GenericPageManager(TidalClient client, string path, int itemsPerPage, List<KeyValuePair<string, string>> queryString = null) : base(client, path, itemsPerPage, queryString)
        {

        }

        public IEnumerable<T> SelectByTypeAndCast<T>(string type)
        {
            return from item in LastPageRequested.Items
                   where item.Type == type
                   select item.Item.ToObject<T>();
        }

    }
}
