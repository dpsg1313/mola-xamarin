using Akavache;
using MolaApp.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MolaApp.Api
{
    public class ProfileApi : AbstractGetObjectApi<ProfileModel>, IProfileApi
    {
        public ProfileApi(HttpClient httpClient) : base(httpClient)
        {

        }

        protected override string ObjectName()
        {
            return "profile";
        }

        public async Task<bool> UpdateAsync(ProfileModel profile)
        {
            string path = ObjectName();
            HttpContent content = CreateJsonContent(profile);

            HttpResponseMessage response = await client.PutAsync(path, content);
            if (response.IsSuccessStatusCode)
            {
                string key = ObjectName() + "_" + profile.Id;
                var cache = BlobCache.LocalMachine;
                cache.InsertObject<ProfileModel>(key, profile);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
