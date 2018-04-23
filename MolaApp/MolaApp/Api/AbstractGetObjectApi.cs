using Akavache;
using MolaApp.Model;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace MolaApp.Api
{
    abstract public class AbstractGetObjectApi<T> : AbstractApi, IGetObjectApi<T> where T : IModel
    {

        public AbstractGetObjectApi(HttpClient httpClient) : base(httpClient)
        {

        }

        abstract protected string ObjectName();

        private async Task<T> GetRemoteAsync(string id)
        {
            string path = ObjectName() + "/" + id;
            HttpResponseMessage response = await client.GetAsync(path);
            Debug.WriteLine("API Response Status: " + response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("API Response json: " + json);
                T profile = JsonConvert.DeserializeObject<T>(json);
                return profile;
            }
            return default(T);
        }

        public IObservable<T> Get(string id)
        {
            string key = ObjectName() + "_" + id;
            var cache = BlobCache.LocalMachine;
            return cache.GetAndFetchLatest(
                key,
                () => GetRemoteAsync(id),
                offset =>
                {
                    TimeSpan elapsed = DateTimeOffset.Now - offset;
                    return elapsed > new TimeSpan(hours: 0, minutes: 1, seconds: 0);
                }
            );
        }

        public async Task<T> GetAsync(string id)
        {
            var cachedPromise = Get(id);

            // The subscribe is necessary, otherwise the object is never fetched
            cachedPromise.Subscribe(subscribed => {});

            return await cachedPromise.FirstOrDefaultAsync();
        }
    }
}
