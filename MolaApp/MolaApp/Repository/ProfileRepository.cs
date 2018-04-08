using MolaApp.Model;
using MolaApp.Api;
using System.Threading.Tasks;

namespace MolaApp.Repository
{
    class ProfileRepository : AbstractRepository<ProfileModel>
    {
        ProfileApi api;

        public ProfileRepository(ProfileApi profileApi)
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
    }

    
}
