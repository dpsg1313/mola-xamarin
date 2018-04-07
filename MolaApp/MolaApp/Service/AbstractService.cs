using MolaApp.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MolaApp.Service
{
    abstract class AbstractService<T> where T : IModel
    {
        protected HttpClient client;

        public AbstractService()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
        }

        async public Task<T> GetAsync(string id)
        {
            string url = GetBaseUrl() + id + ".json";
            Uri uri = new Uri(url);
            System.Diagnostics.Debug.WriteLine("API call");
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                System.Diagnostics.Debug.WriteLine("API Response success");
                string json = await response.Content.ReadAsStringAsync();
                T profile = JsonConvert.DeserializeObject<T>(json);
                return profile;
            }
            return default(T);
        }

        abstract protected string GetBaseUrl();
    }
}
