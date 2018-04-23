using MolaApp.Model;
using System;
using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace MolaApp.Api.Stub
{
    class ProfileApi : IProfileApi
    {
        ConcurrentDictionary<string, ProfileModel> dict = new ConcurrentDictionary<string, ProfileModel>();

        public ProfileApi()
        {
            dict.TryAdd("9354642e-2c70-48e5-9540-d9a00c4a410d", new ProfileModel("9354642e-2c70-48e5-9540-d9a00c4a410d")
            {
                Firstname = "Florian",
                Lastname = "Kick",
                Residence = "Unterhaching",
                FavouriteStage = "Jungpfadfinder",
                FunctionId = "2-2",
                WoodbadgeCount = 2
            });

            dict.TryAdd("7fc57f9e-3a2e-4361-8878-0539db16bb25", new ProfileModel("7fc57f9e-3a2e-4361-8878-0539db16bb25")
            {
                Firstname = "Max",
                Lastname = "Mustermann",
                Residence = "Hinterhugelhapfing",
                FavouriteStage = "Pfadfinder",
                FunctionId = "1-1",
                WoodbadgeCount = 0
            });

            dict.TryAdd("478f7997-3b2d-42c5-b877-d24c21c2a960", new ProfileModel("478f7997-3b2d-42c5-b877-d24c21c2a960")
            {
                Firstname = "Gutemiene",
                Lastname = "Mecklenburger-Mühlbauer",
                Residence = "Friedrichshafen am Bodensee",
                FavouriteStage = "Rover",
                FunctionId = "3-4",
                WoodbadgeCount = 3
            });
        }

        public IObservable<ProfileModel> Get(string id)
        {
            ProfileModel model = null;
            dict.TryGetValue(id, out model);
            return Observable.Return(model);
        }

        public async Task<ProfileModel> GetAsync(string id)
        {
            ProfileModel model = null;
            dict.TryGetValue(id, out model);
            return model;
        }

        public async Task<bool> UpdateAsync(ProfileModel profile)
        {
            dict.AddOrUpdate(profile.Id, profile, (key, oldValue) => profile);
            return true;
        }
    }
}
