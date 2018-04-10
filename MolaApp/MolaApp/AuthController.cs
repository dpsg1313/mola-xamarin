using MolaApp.Api;
using MolaApp.Model;
using Newtonsoft.Json;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MolaApp
{
    public class AuthController
    {
        private const string LOGIN_FOLDER = "login";
        private const string CURRENT_LOGIN_FILE = "current.json";

        public enum State { LoggedIn, LoggedOut};

        public State LoginState { get; private set; }

        public AuthToken AuthToken { get; private set; }

        public UserModel User { get; private set; }

        IUserApi api;

        IFolder loginFolder;

        public AuthController(IUserApi userApi)
        {
            LoginState = State.LoggedOut;
            api = userApi;
        }

        async public Task InitAsync()
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            loginFolder = await rootFolder.CreateFolderAsync(LOGIN_FOLDER, CreationCollisionOption.OpenIfExists);

            await loadCredentialsAsync();
            if(User != null && User.Password != null)
            {
                await LoginAsync(User, true);
            }
        }

        async public Task<bool> LoginAsync(UserModel credentials, bool saveLogin)
        {
            AuthToken authToken = await api.GetTokenAsync(credentials);
            if (authToken != null)
            {
                AuthToken = authToken;

                UserModel saveCredentials = new UserModel(credentials.Id);
                saveCredentials.Email = credentials.Email;
                if (!saveLogin)
                {
                    saveCredentials.Password = credentials.Password;
                }
                await SaveCredentialsAsync(saveCredentials);

                User = credentials;
                LoginState = State.LoggedIn;

                return true;
            }
            return false;
        }

        private async Task loadCredentialsAsync()
        {
            User = null;
            ExistenceCheckResult res = await loginFolder.CheckExistsAsync(CURRENT_LOGIN_FILE);
            if (res.Equals(ExistenceCheckResult.FileExists))
            {
                IFile file = await loginFolder.GetFileAsync(CURRENT_LOGIN_FILE);
                string json = await file.ReadAllTextAsync();
                User = JsonConvert.DeserializeObject<UserModel>(json);
            }
        }

        private async Task SaveCredentialsAsync(UserModel user)
        {
            string json = JsonConvert.SerializeObject(user);
            IFile file = await loginFolder.CreateFileAsync(CURRENT_LOGIN_FILE, CreationCollisionOption.OpenIfExists);
            await file.WriteAllTextAsync(json);
        }
    }
}
