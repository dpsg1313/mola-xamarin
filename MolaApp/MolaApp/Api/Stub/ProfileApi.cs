using MolaApp.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
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
                Name = "Florian Kick",
                Residence = "Unterhaching",
                FavouriteStage = "Jupfis",
                Function = "BeVo",
                WoodbadgeCount = 2
            });

            dict.TryAdd("7fc57f9e-3a2e-4361-8878-0539db16bb25", new ProfileModel("7fc57f9e-3a2e-4361-8878-0539db16bb25")
            {
                Name = "Max Mustermann",
                Residence = "Hinterhugelhapfing",
                FavouriteStage = "Pfadis",
                Function = "Leiter",
                WoodbadgeCount = 0
            });

            dict.TryAdd("478f7997-3b2d-42c5-b877-d24c21c2a960", new ProfileModel("478f7997-3b2d-42c5-b877-d24c21c2a960")
            {
                Name = "Gutemiene Mecklenburger-Mühlbauer",
                Residence = "Friedrichshafen am Bodensee",
                FavouriteStage = "Rover",
                Function = "Diözesanreferent*in",
                WoodbadgeCount = 3
            });
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
