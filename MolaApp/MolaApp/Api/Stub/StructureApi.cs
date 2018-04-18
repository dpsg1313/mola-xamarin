using MolaApp.Model;
using Newtonsoft.Json;
using PCLStorage;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MolaApp.Api.Stub
{
    public class StructureApi : IStructureApi
    {
        private const string RESOURCE_FILE = "MolaApp.Api.Stub.dpsg_structure.json";

        private DpsgStructure structure = null;

        public async Task InitAsync()
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(RESOURCE_FILE))
            using (StreamReader reader = new StreamReader(stream))
            {
                string json = reader.ReadToEnd();
                structure = JsonConvert.DeserializeObject<DpsgStructure>(json);
            }
        }

        public async Task<DpsgStructure> GetAsync()
        {
            return structure;
        }
    }
}
