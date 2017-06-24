using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Athame.PluginAPI.Service;
using AthamePlugin.Tidal.InternalApi.Models;

namespace AthamePlugin.Tidal.InternalApi
{
    public class MetadataHelpers
    {
        public static Metadata ExplicitMetadata(bool isExplicit)
        {
            return new Metadata
            {
                CanDisplay = true,
                IsFlag = true,
                Name = "Explicit",
                Value = isExplicit.ToString()
            };
        }

        public static Metadata MasterMetadata(StreamingQuality quality)
        {
            return new Metadata
            {
                CanDisplay = true,
                IsFlag = true,
                Name = "Master",
                Value = (quality == StreamingQuality.HiRes).ToString()
            };
        }
    }
}
