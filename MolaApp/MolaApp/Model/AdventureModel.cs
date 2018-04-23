using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MolaApp.Model
{
    public class AdventureModel : IModel
    {
        [JsonProperty("id")]
        public string Id { get; }

        [JsonProperty("confirmed")]
        public bool Confirmed { get; set; }

        [JsonProperty("withUserId")]
        public String WithUserId { get; set; }

        [JsonProperty("imageId")]
        public string ImageId { get; set; }

        [JsonProperty("withConfirmed")]
        public bool WithConfirmed { get; set; }

        public AdventureModel(string id)
        {
            Id = id;
        }
    }
}
