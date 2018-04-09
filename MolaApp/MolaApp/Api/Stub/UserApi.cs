using MolaApp.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MolaApp.Api.Stub
{
    public class UserApi : IUserApi
    {
        public async Task<UserModel> GetAsync(string id)
        {
            UserModel model = new UserModel(id);
            return model;
        }

        public async Task CreateAsync(UserModel model)
        {
            
        }

        public async Task<AuthToken> GetTokenAsync(UserModel user)
        {
            string token = "testtoken";
            DateTimeOffset expires = DateTimeOffset.Now.AddDays(5);
            return new AuthToken(token, expires);
        }
    }
}
