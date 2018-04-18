using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MolaApp.Model
{
    public class Diocese
    {
        [JsonProperty("Name")]
        public string Name { get; protected set; }

        [JsonProperty("Id")]
        public string Id { get; protected set; }

        [JsonProperty("HasRegions")]
        public bool HasRegions { get; protected set; }

        [JsonProperty("Regions")]
        public readonly IDictionary<string, Region> Regions;

        [JsonProperty("Tribes")]
        public readonly IDictionary<string, Tribe> Tribes;
    }
}
