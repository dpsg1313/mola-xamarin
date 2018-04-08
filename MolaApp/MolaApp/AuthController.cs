using MolaApp.Api;
using MolaApp.Model;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MolaApp
{
    public class AuthController
    {
        public enum State { LoggedIn, LoggedOut};

        public State LoginState { get; }

        public UserModel User { get; }

        public AuthToken AuthToken { get; private set; }

        IUserApi api;

        public AuthController(IUserApi userApi)
        {
            LoginState = State.LoggedOut;
            api = userApi;
        }

        async public Task InitAsync()
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            IFolder folder = await rootFolder.CreateFolderAsync("login", CreationCollisionOption.OpenIfExists);
        }

        async public Task<bool> LoginAsync(UserModel credentials)
        {
            AuthToken authToken = await api.GetTokenAsync(credentials);
            if (authToken != null)
            {
                AuthToken = authToken;
                return true;
            }
            return false;
        }
    }
}
