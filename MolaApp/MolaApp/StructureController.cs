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
    public class StructureController
    {
        private const string FOLDER = "structure";
        private const string FILE = "current.json";

        IStructureApi api;

        IFolder folder;

        public DpsgStructure Structure { get; private set; }

        public StructureController(IStructureApi structureApi)
        {
            api = structureApi;
        }

        async public Task InitAsync()
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            folder = await rootFolder.CreateFolderAsync(FOLDER, CreationCollisionOption.OpenIfExists);

            await loadStructureAsync();
        }

        private async Task loadStructureAsync()
        {
            Structure = null;
            ExistenceCheckResult res = await folder.CheckExistsAsync(FOLDER);
            if (res.Equals(ExistenceCheckResult.FileExists))
            {
                IFile file = await folder.GetFileAsync(FILE);
                string json = await file.ReadAllTextAsync();
                Structure = JsonConvert.DeserializeObject<DpsgStructure>(json);
            }

            if(Structure == null)
            {
                DpsgStructure structure = await api.GetAsync();
                if (structure != null)
                {
                    Structure = structure;
                    await SaveStructureAsync(structure);
                }
            }
        }

        private async Task SaveStructureAsync(DpsgStructure structure)
        {
            string json = JsonConvert.SerializeObject(structure);
            IFile file = await folder.CreateFileAsync(FILE, CreationCollisionOption.OpenIfExists);
            await file.WriteAllTextAsync(json);
        }
    }
}
