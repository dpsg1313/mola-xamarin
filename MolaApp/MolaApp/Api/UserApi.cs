using MolaApp.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
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

        public UserApi(HttpClient httpClient) : base(httpClient)
        {

        }

        protected override string ObjectName()
        {
            return "user";
        }

        public async Task CreateAsync(UserModel model)
        {
            string path = ObjectName();
            HttpContent content = CreateJsonContent(model);

            HttpResponseMessage response = await client.PostAsync(path, content);
            if (response.IsSuccessStatusCode)
            {
                return;
            }
            else if(response.StatusCode == HttpStatusCode.Conflict)
            {
                throw new ConflictException();
            }
            else
            {
                throw new Exception("Unexpected failure!");
            }
        }

        public async Task<AuthToken> GetTokenAsync(UserModel credentials)
        {
            string path = "login";
            HttpContent content = CreateJsonContent(credentials);

            HttpResponseMessage response = await client.PostAsync(path, content);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                string token = JsonConvert.DeserializeObject<string>(json);
                return new AuthToken(credentials.Id, token);
            }
            return null;
        }
    }
}
