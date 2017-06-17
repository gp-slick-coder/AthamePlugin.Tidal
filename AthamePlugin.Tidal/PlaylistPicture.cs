using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Athame.PluginAPI.Service;
using AthamePlugin.Tidal.InternalApi;

namespace AthamePlugin.Tidal
{
    public class PlaylistPicture : Picture
    {
        internal readonly string PictureId;

        internal PlaylistPicture(string pictureId)
        {
            PictureId = pictureId;
        }

        public override async Task<byte[]> GetLargestVersionAsync()
        {
            var url = PictureUrlResolver.ResolveUrl(PictureId, 1080, 720);
            return await new HttpClient().GetByteArrayAsync(url);
        }

        public override async Task<byte[]> GetThumbnailVersionAsync()
        {
            return await new HttpClient().GetByteArrayAsync(
                PictureUrlResolver.ResolveUrl(PictureId, 320, 214));
        }

        public override bool IsThumbnailAvailable => true;
    }
}
