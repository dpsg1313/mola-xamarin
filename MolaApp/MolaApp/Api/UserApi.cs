using MolaApp.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MolaApp.Api
{
    public class UserApi : AbstractApi<UserModel>, IUserApi
    {
        public static string ConflictFieldEmail = "email";
        public static string ConflictFieldId = "id";

        string baseUrl = "http://encala.de/";

        protected override string GetBaseUrl()
        {
            return baseUrl;
        }

        public async Task CreateAsync(UserModel model)
        {
            string url = GetBaseUrl();
            Uri uri = new Uri(url);

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model));

            HttpResponseMessage response = await client.PostAsync(uri, content);
            if (response.IsSuccessStatusCode)
            {
                return;
            }
            else if(response.StatusCode == HttpStatusCode.Conflict)
            {
                string json = await response.Content.ReadAsStringAsync();
                JObject o = JObject.Parse(json);
                if (o.ContainsKey("field"))
                {
                    string field = o.GetValue("field").ToString();
                    throw new ConflictException(field);
                }
            }
            throw new Exception("Unexpected failure!");
        }

        public async Task<AuthToken> GetTokenAsync(UserModel credentials)
        {
            string url = GetBaseUrl() + "/login";
            Uri uri = new Uri(url);

            HttpContent content = new StringContent(JsonConvert.SerializeObject(credentials));

            HttpResponseMessage response = await client.PostAsync(uri, content);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                JObject o = JObject.Parse(json);
                if (o.ContainsKey("token") && o.ContainsKey("expires"))
                {
                    string token = o.GetValue("token").ToString();
                    DateTimeOffset expires = DateTimeOffset.Parse(o.GetValue("expires").ToString());
                    return new AuthToken(credentials.Id, token, expires);
                }
            }
            return null;
        }
    }
}
