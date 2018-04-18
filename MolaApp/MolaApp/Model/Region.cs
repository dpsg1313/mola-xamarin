using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MolaApp.Model
{
    public class Region
    {
        [JsonProperty("Name")]
        public string Name { get; protected set; }

        [JsonProperty("Id")]
        public string Id { get; protected set; }

        [JsonProperty("DioceseId")]
        public string DioceseId { get; protected set; }

        [JsonProperty("Tribes")]
        public readonly IDictionary<string, Tribe> Tribes;
    }
}
