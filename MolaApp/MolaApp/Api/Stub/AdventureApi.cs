using MolaApp.Model;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace MolaApp.Api.Stub
{
    class AdventureApi : IAdventureApi
    {
        public IObservable<AdventureModel> Get(string id)
        {
            return Observable.Return(new AdventureModel(id));
        }

        public async Task<AdventureModel> GetAsync(string id)
        {
            AdventureModel model = new AdventureModel(id);
            return model;
        }
    }
}
