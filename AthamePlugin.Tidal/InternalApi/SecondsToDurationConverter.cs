using System;
using Newtonsoft.Json;

namespace AthamePlugin.Tidal.InternalApi
{
    internal class SecondsToDurationConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType != typeof(int)) return null;
            return new TimeSpan(0, 0, (int)existingValue);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(int);
        }
    }
}
