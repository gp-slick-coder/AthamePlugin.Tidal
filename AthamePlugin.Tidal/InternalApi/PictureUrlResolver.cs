using System;

namespace AthamePlugin.Tidal.InternalApi
{
    public static class PictureUrlResolver
    {
        private const string BaseUrl = "https://resources.tidal.com/images/{0}/{1}x{2}.jpg";

        public static string ResolveUrl(string id, int width, int height)
        {
            return String.Format(BaseUrl, id.Replace('-', '/'), width, height);
        }
    }
}
