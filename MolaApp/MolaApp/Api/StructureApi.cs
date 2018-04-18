using MolaApp.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MolaApp.Api
{
    public class StructureApi : IStructureApi
    {
        string baseUrl = "http://encala.de/";

        protected HttpClient client;

        public StructureApi()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
        }

        public async Task<DpsgStructure> GetAsync()
        {
            Uri uri = new Uri(baseUrl);
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                DpsgStructure structure = JsonConvert.DeserializeObject<DpsgStructure>(json);
                return structure;
            }
            return null;
        }
    }
}
