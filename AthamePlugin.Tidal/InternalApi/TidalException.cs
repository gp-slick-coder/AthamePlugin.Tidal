using System;
using Newtonsoft.Json;

namespace AthamePlugin.Tidal.InternalApi
{
    public class TidalException : Exception
    {

        internal TidalException(TidalError error) : base($"{error.Status}/{error.SubStatus}: {error.UserMessage}")
        {

        }
    }
}
