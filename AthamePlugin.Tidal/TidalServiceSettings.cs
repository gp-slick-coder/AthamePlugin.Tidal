using OpenTidl.Enums;
using OpenTidl.Models;

namespace AthamePlugin.Tidal
{
    public class TidalServiceSettings
    {
        public SoundQuality StreamQuality { get; set; }
        public bool AppendVersionToTrackTitle { get; set; }
        public bool DontAppendAlbumVersion { get; set; }
        public bool UseOfflineUrl { get; set; }
        public string SessionToken { get; set; }
        public UserModel User { get; set; }
        public TidalServiceSettings()
        {
            StreamQuality = SoundQuality.HIGH;
            AppendVersionToTrackTitle = true;
            DontAppendAlbumVersion = true;
            UseOfflineUrl = true;
        }
    }
}
