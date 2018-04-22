using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MolaApp.Model
{
    public class Region
    {
        [JsonProperty("name")]
        public string Name { get; protected set; }

        [JsonProperty("id")]
        public string Id { get; protected set; }

        [JsonProperty("dioceseId")]
        public string DioceseId { get; protected set; }

        [JsonProperty("tribes")]
        public readonly IDictionary<string, Tribe> Tribes;
    }
}
