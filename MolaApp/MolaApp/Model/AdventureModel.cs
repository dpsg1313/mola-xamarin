using System;
using System.Collections.Generic;
using System.Text;

namespace MolaApp.Model
{
    public class AdventureModel : IModel
    {
        public string Id { get; }

        public ISet<String> UserIds { get; set; }
        public string FotoUrl { get; set; }

        public AdventureModel(string id)
        {
            Id = id;
        }
    }
}
