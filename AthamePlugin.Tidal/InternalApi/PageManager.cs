using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AthamePlugin.Tidal.InternalApi.Models;

namespace AthamePlugin.Tidal.InternalApi
{
    public abstract class PageManager<T>
    {
        protected PageManager(int itemsPerPage)
        {
            ItemsPerPage = itemsPerPage;
        }

        public List<T> AllItems { get; set; }

        public int ItemsPerPage { get; }

        public int TotalNumberOfItems { get; set; }

        public PaginatedList<T> LastPageRequested { get; set; }

        public abstract Task<PaginatedList<T>> LoadNextPageAsync();
    }
}
