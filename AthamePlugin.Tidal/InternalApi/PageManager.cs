using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AthamePlugin.Tidal.InternalApi.Models;

namespace AthamePlugin.Tidal.InternalApi
{
    public class PageManager<T>
    {
        public PageManager(int limit)
        {
            Limit = limit;
        }

        public List<T> AllItems { get; set; }

        public int Limit { get; set; }

        public int NextOffset { get; set; }

        public int TotalItems { get; set; }

        public void Append(PaginatedList<T> page)
        {
            Increment(page);
            AllItems.AddRange(page.Items);
        }

        public void Increment(PaginatedList<T> page)
        {
            TotalItems = page.TotalNumberOfItems;
            NextOffset += page.Limit;
        }
    }
}
