using MolaApp.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MolaApp.Api.Stub
{
    class ProfileApi : IProfileApi
    {
        public async Task<ProfileModel> GetAsync(string id)
        {
            ProfileModel model = new ProfileModel(id);
            return model;
        }
    }
}
