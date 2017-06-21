using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AthamePlugin.Tidal.InternalApi.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ArtistRole
    {
        [EnumMember(Value = "MAIN")]
        Main,
        [EnumMember(Value = "FEATURED")]
        Featured
    }
}
