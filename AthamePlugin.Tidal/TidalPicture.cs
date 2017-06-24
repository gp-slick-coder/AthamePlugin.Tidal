using System.Net.Http;
using System.Threading.Tasks;
using Athame.PluginAPI.Downloader;
using Athame.PluginAPI.Service;
using AthamePlugin.Tidal.InternalApi;

namespace AthamePlugin.Tidal
{
    public class TidalPicture : Picture
    {
        internal const int ThumbnailSize = 300;
        internal const int FullSize = 1280;

        internal readonly string PictureId;

        internal TidalPicture(string pictureId)
        {
            PictureId = pictureId;
            FileType = MediaFileTypes.JpegImage;
        }

        public override async Task<byte[]> GetLargestVersionAsync()
        {
            var url = PictureUrlResolver.ResolveUrl(PictureId, FullSize, FullSize);
            return await new HttpClient().GetByteArrayAsync(url);
        }

        public override async Task<byte[]> GetThumbnailVersionAsync()
        {
            return await new HttpClient().GetByteArrayAsync(
                PictureUrlResolver.ResolveUrl(PictureId, ThumbnailSize, ThumbnailSize));
        }

        public override bool IsThumbnailAvailable => true;
    }
}
