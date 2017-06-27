using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Athame.PluginAPI.Downloader;
using AthamePlugin.Tidal.InternalApi.Decryption;
using AthamePlugin.Tidal.InternalApi.Models;
using Newtonsoft.Json;

namespace AthamePlugin.Tidal
{
    public class TidalTrackFile : TrackFile
    {
        public FileKey FileKey { get; set; }
    }
}
