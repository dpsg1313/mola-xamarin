using System;
using System.Collections.Generic;
using System.Text;

namespace MolaApp.Model
{
    public class ProfileModel : IModel
    {
        public string Id { get; }

        public string Name { get; set; }
        public string TribeId { get; set; }
        public string RegionId { get; set; }
        public string DioceseId { get; set; }
        public string FunctionId { get; set; }

        public string Phone { get; set; }
        public string Mail { get; set; }
        public string Residence { get; set; }
        public string FavouriteStage { get; set; }
        public string Description { get; set; }

        public int WoodbadgeCount { get; set; }

        public ProfileModel(string id)
        {
            Id = id;
        }
    }
}
