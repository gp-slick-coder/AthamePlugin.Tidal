using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AthamePlugin.Tidal.InternalApi.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StreamingQuality
    {
        [EnumMember(Value = "LOW")]
        Low,
        [EnumMember(Value = "HIGH")]
        High,
        [EnumMember(Value = "LOSSLESS")]
        Lossless,
        [EnumMember(Value = "HI_RES")]
        HiRes
    }
}