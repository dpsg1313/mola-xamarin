using MolaApp.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MolaApp
{
    class ProfileService
    {
        HttpClient client;
        string baseUrl = "http://encala.de/";

        public ProfileService()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
        }

        async public Task<ProfileModel> GetAsync(string id)
        {
            string url = baseUrl + id + ".json";
            var uri = new Uri(url);
            System.Diagnostics.Debug.WriteLine("API call");
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                System.Diagnostics.Debug.WriteLine("API Response success");
                var json = await response.Content.ReadAsStringAsync();
                ProfileModel profile = JsonConvert.DeserializeObject<ProfileModel>(json);
                return profile;
            }
            return null;
        }

    }
}
