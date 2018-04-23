using Akavache;
using MolaApp.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolaApp.Api
{
    class AdventureApi : AbstractApi, IAdventureApi
    {
        const string PATH_POST = "adventure";
        const string PATH_GET = "adventures";

        public AdventureApi(HttpClient httpClient) : base(httpClient)
        {

        }

        private async Task<List<AdventureModel>> GetRemoteAsync(string profileId)
        {
            string path = PATH_GET + "/" + profileId;
            HttpResponseMessage response = await client.GetAsync(path);
            Debug.WriteLine("API Response Status: " + response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("API Response json: " + json);
                IDictionary<string, AdventureModel> adventures = JsonConvert.DeserializeObject<IDictionary<string, AdventureModel>>(json);
                return adventures.Values.ToList();
            }
            return null;
        }

        public IObservable<List<AdventureModel>> Get(string profileId)
        {
            string key = PATH_GET + "_" + profileId;
            var cache = BlobCache.LocalMachine;
            return cache.GetAndFetchLatest(
                key,
                () => GetRemoteAsync(profileId),
                offset =>
                {
                    TimeSpan elapsed = DateTimeOffset.Now - offset;
                    return elapsed > new TimeSpan(hours: 0, minutes: 1, seconds: 0);
                }
            );
        }

        public async Task<List<AdventureModel>> GetAsync(string profileId)
        {
            var cachedPromise = Get(profileId);

            // The subscribe is necessary, otherwise the object is never fetched
            cachedPromise.Subscribe(subscribed => { });

            return await cachedPromise.FirstOrDefaultAsync();
        }

        public async Task<bool> AddAsync(string profileId, int myPoints, int otherPoints)
        {
            NewAdventureModel model = new NewAdventureModel
            {
                ImageId = "",
                MyPoints = myPoints,
                OtherPoints = otherPoints
            };

            string path = PATH_POST + "/" + profileId;
            HttpContent content = CreateJsonContent(model);

            HttpResponseMessage response = await client.PostAsync(path, content);
            return response.IsSuccessStatusCode;
        }

        
    }
}
