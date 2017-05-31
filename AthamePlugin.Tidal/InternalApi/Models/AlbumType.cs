using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AthamePlugin.Tidal.InternalApi.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AlbumType
    {
        [EnumMember(Value = "SINGLE")]
        Single,
        [EnumMember(Value = "ALBUM")]
        Album,
        [EnumMember(Value = "EP")]
        Ep
    }
}
