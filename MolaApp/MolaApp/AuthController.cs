using MolaApp.Api;
using MolaApp.Model;
using Newtonsoft.Json;
using PCLStorage;
using System;
using System.Threading.Tasks;

namespace MolaApp
{
    public class AuthController
    {
        private const string TOKEN_FOLDER = "token";
        private const string CURRENT_TOKEN_FILE = "current.json";
        private const string TOKEN_PROPERTY = "AuthToken";

        public bool IsLoggedIn { get; private set; }

        public AuthToken AuthToken { get; private set; }

        IUserApi api;

        IFolder loginFolder;

        public event EventHandler TokenChanged;

        public AuthController(IUserApi userApi)
        {
            IsLoggedIn = false;
            api = userApi;
        }

        private void NotifyTokenChanged()
        {
            TokenChanged?.Invoke(this, null);
        }

        async public Task InitAsync()
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            loginFolder = await rootFolder.CreateFolderAsync(TOKEN_FOLDER, CreationCollisionOption.OpenIfExists);

            await loadTokenAsync();
        }

        async public Task<bool> LoginAsync(UserModel credentials)
        {
            AuthToken authToken = await api.GetTokenAsync(credentials);
            if (authToken != null)
            {
                AuthToken = authToken;
                await SaveTokenAsync(authToken);

                IsLoggedIn = true;

                NotifyTokenChanged();

                return true;
            }
            return false;
        }

        async public Task<bool> LogoutAsync()
        {
            if (AuthToken != null)
            {
                await DeleteTokenAsync();
                AuthToken = null;
                IsLoggedIn = false;

                NotifyTokenChanged();

                return true;
            }
            return false;
        }

        private async Task loadTokenAsync()
        {
            AuthToken = null;
            ExistenceCheckResult res = await loginFolder.CheckExistsAsync(CURRENT_TOKEN_FILE);
            if (res.Equals(ExistenceCheckResult.FileExists))
            {
                IFile file = await loginFolder.GetFileAsync(CURRENT_TOKEN_FILE);
                string json = await file.ReadAllTextAsync();
                AuthToken = JsonConvert.DeserializeObject<AuthToken>(json);
                if(AuthToken != null)
                {
                    IsLoggedIn = true;
                }
                NotifyTokenChanged();
            }
        }

        private async Task SaveTokenAsync(AuthToken authToken)
        {
            string json = JsonConvert.SerializeObject(authToken);
            IFile file = await loginFolder.CreateFileAsync(CURRENT_TOKEN_FILE, CreationCollisionOption.OpenIfExists);
            await file.WriteAllTextAsync(json);
        }

        private async Task DeleteTokenAsync()
        {
            ExistenceCheckResult res = await loginFolder.CheckExistsAsync(CURRENT_TOKEN_FILE);
            if (res.Equals(ExistenceCheckResult.FileExists))
            {
                IFile file = await loginFolder.GetFileAsync(CURRENT_TOKEN_FILE);
                await file.DeleteAsync();
            }
        }
    }
}
