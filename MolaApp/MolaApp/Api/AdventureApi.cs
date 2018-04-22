using MolaApp.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MolaApp.Api
{
    class AdventureApi : AbstractApi<AdventureModel>, IAdventureApi
    {
        public AdventureApi(HttpClient httpClient) : base(httpClient)
        {

        }

        protected override string ObjectName()
        {
            return "adventure";
        }
    }
}
