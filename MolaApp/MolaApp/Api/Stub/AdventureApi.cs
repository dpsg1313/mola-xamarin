using MolaApp.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MolaApp.Api.Stub
{
    class AdventureApi : IAdventureApi
    {
        public async Task<AdventureModel> GetAsync(string id)
        {
            AdventureModel model = new AdventureModel(id);
            return model;
        }
    }
}
