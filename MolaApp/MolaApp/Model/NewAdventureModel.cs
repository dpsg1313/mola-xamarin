using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MolaApp.Model
{
    public class NewAdventureModel
    {
        [JsonProperty("imageId")]
        public string ImageId { get; set; }

        [JsonProperty("myPoints")]
        public int MyPoints { get; set; }

        [JsonProperty("otherPoints")]
        public int OtherPoints { get; set; }
    }
}
