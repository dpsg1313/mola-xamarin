using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MolaApp.Model
{
    public class Tribe
    {
        [JsonProperty("name")]
        public string Name { get; protected set; }

        [JsonProperty("id")]
        public string Id { get; protected set; }

        [JsonProperty("regionId")]
        public string RegionId { get; protected set; }

        [JsonProperty("dioceseId")]
        public string DioceseId { get; protected set; }
    }
}
