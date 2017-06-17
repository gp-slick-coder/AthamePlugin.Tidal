using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AthamePlugin.Tidal.InternalApi.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TidalAlbumType
    {
        [EnumMember(Value = "SINGLE")]
        Single,
        [EnumMember(Value = "ALBUM")]
        Album,
        [EnumMember(Value = "EP")]
        Ep
    }
}
