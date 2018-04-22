using MolaApp.Model;
using System;
using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace MolaApp.Api.Stub
{
    public class UserApi : IUserApi
    {
        ConcurrentDictionary<string, UserModel> dict = new ConcurrentDictionary<string, UserModel>();

        public UserApi()
        {
            dict.TryAdd("9354642e-2c70-48e5-9540-d9a00c4a410d", new UserModel("9354642e-2c70-48e5-9540-d9a00c4a410d")
            {
                Password = null
            });

            dict.TryAdd("7fc57f9e-3a2e-4361-8878-0539db16bb25", new UserModel("7fc57f9e-3a2e-4361-8878-0539db16bb25")
            {
                Password = "max"
            });

            dict.TryAdd("478f7997-3b2d-42c5-b877-d24c21c2a960", new UserModel("478f7997-3b2d-42c5-b877-d24c21c2a960")
            {
                Password = "gg"
            });
        }

        public IObservable<UserModel> Get(string id)
        {
            UserModel model = null;
            dict.TryGetValue(id, out model);
            return Observable.Return(model);
        }

        public async Task<UserModel> GetAsync(string id)
        {
            UserModel model = null;
            dict.TryGetValue(id, out model);
            return model;
        }

        public async Task CreateAsync(UserModel user)
        {
            if (dict.ContainsKey(user.Id))
            {
                throw new ConflictException();
            }
            dict.TryAdd(user.Id, user);
        }

        public async Task<AuthToken> GetTokenAsync(UserModel credentials)
        {
            if(!dict.ContainsKey(credentials.Id))
            {
                return null;
            }

            UserModel user = null;
            if(dict.TryGetValue(credentials.Id, out user))
            {
                if (user.Password == credentials.Password)
                {
                    string token = "testtoken";
                    return new AuthToken(user.Id, token);
                }
            }
            
            return null;
        }
    }
}
