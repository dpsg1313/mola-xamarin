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
        UserModel model;

        public async Task<UserModel> GetAsync(string id)
        {
            return model;
        }

        public async Task CreateAsync(UserModel user)
        {
            model = user;
        }

        public async Task<AuthToken> GetTokenAsync(UserModel user)
        {
            if(model == null)
            {
                return null;
            }

            if (user.Id == model.Id && user.Password == model.Password)
            {
                string token = "testtoken";
                DateTimeOffset expires = DateTimeOffset.Now.AddDays(5);
                return new AuthToken(user.Id, token, expires);
            }
            return null;
        }
    }
}
