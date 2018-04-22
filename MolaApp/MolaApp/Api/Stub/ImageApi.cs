using MolaApp.Model;
using System;
using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace MolaApp.Api.Stub
{
    class ImageApi : IImageApi
    {
        ConcurrentDictionary<string, ImageModel> dict = new ConcurrentDictionary<string, ImageModel>();

        public IObservable<ImageModel> Get(string id)
        {
            ImageModel model = null;
            dict.TryGetValue(id, out model);
            return Observable.Return(model);
        }

        async public Task<ImageModel> GetAsync(string id)
        {
            ImageModel model = null;
            dict.TryGetValue(id, out model);
            return model;
        }

        public async Task<bool> PutAsync(ImageModel image)
        {
            dict.AddOrUpdate(image.Id, image, (key, oldValue) => image);
            return true;
        }
    }
}
