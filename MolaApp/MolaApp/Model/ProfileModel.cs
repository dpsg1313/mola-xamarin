using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MolaApp.Model
{
    public class ProfileModel : IModel
    {
        [JsonProperty("id")]
        public string Id { get; }

        [JsonProperty("firstname")]
        public string Firstname { get; set; }

        [JsonProperty("lastname")]
        public string Lastname { get; set; }

        [JsonProperty("tribeId")]
        public string TribeId { get; set; }

        [JsonProperty("regionId")]
        public string RegionId { get; set; }

        [JsonProperty("dioceseId")]
        public string DioceseId { get; set; }

        [JsonProperty("functionId")]
        public string FunctionId { get; set; }


        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("email")]
        public string Mail { get; set; }

        [JsonProperty("residence")]
        public string Residence { get; set; }

        [JsonProperty("favouriteStage")]
        public string FavouriteStage { get; set; }

        [JsonProperty("relationshipStatus")]
        public string RelationshipStatus { get; set; }


        [JsonProperty("woodbadgeCount")]
        public int WoodbadgeCount { get; set; }

        [JsonProperty("georgesPoints")]
        public int GeorgesPoints { get; set; }


        [JsonProperty("imageId")]
        public string ImageId { get; set; }

        [JsonIgnore]
        public string Name
        {
            get
            {
                return Firstname + " " + Lastname;
            }
        }

        public ProfileModel(string id)
        {
            Id = id;
        }
    }
}
