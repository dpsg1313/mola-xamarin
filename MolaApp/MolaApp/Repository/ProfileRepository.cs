using MolaApp.Model;
using MolaApp.Api;
using System.Threading.Tasks;

namespace MolaApp.Repository
{
    class ProfileRepository : AbstractRepository<ProfileModel>
    {
        IProfileApi api;

        public ProfileRepository(IProfileApi profileApi)
        {
            api = profileApi;
        }

        override protected string GetFolderName()
        {
            return "profiles";
        }

        protected override async Task<ProfileModel> GetFromService(string id)
        {
            return await api.GetAsync(id);
        }

        public async void PutAsync(ProfileModel profile)
        {
            _dict.AddOrUpdate(profile.Id, profile, (key, oldValue) => profile);
            await api.UpdateAsync(profile);
        }
    }

    
}
