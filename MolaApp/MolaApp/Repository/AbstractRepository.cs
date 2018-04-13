using MolaApp.Model;
using Newtonsoft.Json;
using PCLStorage;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace MolaApp.Repository
{
    abstract class AbstractRepository<T> where T : IModel
    {
        protected ConcurrentDictionary<string, T> _dict = new ConcurrentDictionary<string, T>();
        IFolder folder;

        async public Task InitAsync()
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            folder = await rootFolder.CreateFolderAsync(GetFolderName(), CreationCollisionOption.OpenIfExists);
        }

        async public Task<T> GetAsync(string id)
        {
            T value;

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
                value = JsonConvert.DeserializeObject<T>(json);
                return value;
            }

            System.Diagnostics.Debug.WriteLine("Try get profile from API");
            value = await GetFromService(id);
            if (value != null)
            {
                _dict.TryAdd(id, value);
                // TODO save to file
                return value;
            }

            return default(T);
        }

        abstract protected string GetFolderName();

        abstract protected Task<T> GetFromService(string id);
    }

    
}
