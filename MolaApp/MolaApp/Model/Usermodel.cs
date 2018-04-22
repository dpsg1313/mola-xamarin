using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MolaApp.Model
{
    public class UserModel : IModel
    {
        [JsonProperty("id")]
        public string Id { get; }

        [JsonProperty("password")]
        public string Password { get; set; }

        public UserModel(string id)
        {
            Id = id;
        }
    }
}
