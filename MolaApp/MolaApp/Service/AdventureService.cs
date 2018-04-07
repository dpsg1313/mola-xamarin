using MolaApp.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MolaApp.Service
{
    class AdventureService : AbstractService<AdventureModel>
    {
        string baseUrl = "http://encala.de/";

        protected override string GetBaseUrl()
        {
            return baseUrl;
        }
    }
}
