using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MolaApp.Model
{
    public class Diocese
    {
        [JsonProperty("name")]
        public string Name { get; protected set; }

        [JsonProperty("id")]
        public string Id { get; protected set; }

        [JsonProperty("hasRegions")]
        public bool HasRegions { get; protected set; }

        [JsonProperty("regions")]
        public readonly IDictionary<string, Region> Regions;

        [JsonProperty("tribes")]
        public readonly IDictionary<string, Tribe> Tribes;
    }
}
