using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MolaApp.Model
{
    public class Function
    {
        public enum Levels { Bund, Diocese, Region, Tribe };

        [JsonProperty("Id")]
        public string Id { get; protected set; }

        [JsonProperty("Name")]
        public string Name { get; protected set; }

        [JsonProperty("Level")]
        public Levels Level { get; protected set; }
    }
}
