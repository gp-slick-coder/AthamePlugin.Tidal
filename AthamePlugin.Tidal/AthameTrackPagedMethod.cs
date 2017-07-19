using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Athame.PluginAPI.Service;
using AthamePlugin.Tidal.InternalApi.Models;

namespace AthamePlugin.Tidal
{
    public class AthameTrackPagedMethod : PagedMethod<Track>
    {
        private PagedMethod<TidalTrack> tracksPagedMethod;
        private TidalServiceSettings settings;

        public AthameTrackPagedMethod(TidalServiceSettings settings, PagedMethod<TidalTrack> tracksPagedMethod) : base(tracksPagedMethod.ItemsPerPage)
        {
            this.tracksPagedMethod = tracksPagedMethod;
            this.settings = settings;
        }

        public override async Task<IList<Track>> GetNextPageAsync()
        {
            var nextItems = await tracksPagedMethod.GetNextPageAsync();
            return nextItems.Select(tidalTrack => tidalTrack.CreateAthameTrack(settings)).ToList();
        }

        public override int ItemsPerPage => tracksPagedMethod.ItemsPerPage;

        public override IList<Track> AllItems => tracksPagedMethod.AllItems.Select(tidalTrack => tidalTrack.CreateAthameTrack(settings)).ToList();

        public override bool HasMoreItems => tracksPagedMethod.HasMoreItems;

        public override int TotalNumberOfItems => tracksPagedMethod.TotalNumberOfItems;

        public override async Task LoadAllPagesAsync()
        {
            while (HasMoreItems)
            {
                await GetNextPageAsync();
            }
        }
    }
}
