using Akavache;
using MolaApp.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolaApp.Api
{
    public class StructureApi : IStructureApi
    {
        protected HttpClient client;

        public StructureApi(HttpClient httpClient)
        {
            client = httpClient;
        }

        protected string ObjectName()
        {
            return "structure";
        }

        private async Task<DpsgStructure> GetRemoteAsync()
        {
            string path = ObjectName();
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                DpsgStructure structure = JsonConvert.DeserializeObject<DpsgStructure>(json);
                return structure;
            }
            return null;
        }

        public async Task<DpsgStructure> GetAsync()
        {
            DpsgStructure result = null;
            string key = ObjectName();
            var cache = BlobCache.LocalMachine;
            var cachedPromise = cache.GetAndFetchLatest(
                key,
                () => GetRemoteAsync(),
                offset =>
                {
                    TimeSpan elapsed = DateTimeOffset.Now - offset;
                    return elapsed > new TimeSpan(hours: 0, minutes: 1, seconds: 0);
                }
            );

            cachedPromise.Subscribe(subscribedStructure => {
                Debug.WriteLine("Subscribed Object ready");
                result = subscribedStructure;
            });

            result = await cachedPromise.FirstOrDefaultAsync();
            return result;
        }
    }
}
