using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MolaApp.Model
{
    public class Tribe
    {
        [JsonProperty("Name")]
        public string Name { get; protected set; }

        [JsonProperty("Id")]
        public string Id { get; protected set; }

        [JsonProperty("RegionId")]
        public string RegionId { get; protected set; }

        [JsonProperty("DioceseId")]
        public string DioceseId { get; protected set; }
    }
}
