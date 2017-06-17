using AthamePlugin.Tidal.InternalApi.Models;

namespace AthamePlugin.Tidal
{
    public class TidalServiceSettings
    {
        public StreamingQuality StreamQuality { get; set; }
        public bool AppendVersionToTrackTitle { get; set; }
        public bool DontAppendAlbumVersion { get; set; }
        public bool UseOfflineUrl { get; set; }
        public string SessionToken { get; set; }
        public TidalUser User { get; set; }
        public TidalServiceSettings()
        {
            StreamQuality = StreamingQuality.High;
            AppendVersionToTrackTitle = true;
            DontAppendAlbumVersion = true;
            UseOfflineUrl = true;
        }
    }
}
