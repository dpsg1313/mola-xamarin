using MolaApp.Model;
using Newtonsoft.Json;
using PCLStorage;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace MolaApp.Repository
{
    class ProfileRepository
    {
        ConcurrentDictionary<string, ProfileModel> _dict = new ConcurrentDictionary<string, ProfileModel>();
        IFolder folder;
        ProfileService service;

        async public Task InitAsync()
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            folder = await rootFolder.CreateFolderAsync("profiles", CreationCollisionOption.OpenIfExists);
            service = new ProfileService();
        }

        async public Task<ProfileModel> GetAsync(string id)
        {
            ProfileModel value;

            System.Diagnostics.Debug.WriteLine("Try get profile from Dictionary");
            if (_dict.TryGetValue(id, out value))
            {
                return value;
            }

            System.Diagnostics.Debug.WriteLine("Try get profile from file");
            string filename = id + ".json";
            ExistenceCheckResult res = await folder.CheckExistsAsync(filename);
            if (res.Equals(ExistenceCheckResult.FileExists))
            {
                IFile file = await folder.GetFileAsync(filename);
                string json = await file.ReadAllTextAsync();
                value = JsonConvert.DeserializeObject<ProfileModel>(json);
                return value;
            }

            System.Diagnostics.Debug.WriteLine("Try get profile from API");
            value = await service.GetAsync(id);
            if (value != null)
            {
                _dict.TryAdd(id, value);
                return value;
            }

            return null;
        }
    }

    
}
