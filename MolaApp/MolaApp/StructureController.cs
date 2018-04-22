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
        IStructureApi api;


        DpsgStructure structure;
        public DpsgStructure Structure
        {
            get
            {
                return structure;
            }
        }

        public StructureController(IStructureApi structureApi)
        {
            api = structureApi;
        }

        async public Task InitAsync()
        {
            structure = await api.GetAsync();
            if(structure == null)
            {
                throw new Exception("Structure could not be fetched from API!");
            }
        }
    }
}
