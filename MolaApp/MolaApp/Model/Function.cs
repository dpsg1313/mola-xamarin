using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MolaApp.Model
{
    public class Function
    {
        public enum Levels { Bund, Diocese, Region, Tribe };

        [JsonProperty("id")]
        public string Id { get; protected set; }

        [JsonProperty("name")]
        public string Name { get; protected set; }

        [JsonProperty("level")]
        public Levels Level { get; protected set; }
    }
}
