using System;
using Newtonsoft.Json;

namespace AthamePlugin.Tidal.InternalApi.Models
{
    public class TidalUser
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("created")]
        public DateTime Created { get; set; }

        [JsonProperty("picture")]
        public string Picture { get; set; }

        [JsonProperty("newsletter")]
        public bool Newsletter { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("dateOfBirth")]
        public DateTime? DateOfBirth { get; set; }

        [JsonProperty("facebookUid")]
        public ulong FacebookUid { get; set; }
    }

}
