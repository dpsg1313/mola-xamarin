using MolaApp.Model;
using MolaApp.Service;
using System.Threading.Tasks;

namespace MolaApp.Repository
{
    class ProfileRepository : AbstractRepository<ProfileModel>
    {
        ProfileService service;

        override protected string GetFolderName()
        {
            return "profiles";
        }

        override protected void InitSub()
        {
            service = new ProfileService();
        }

        protected override async Task<ProfileModel> GetFromService(string id)
        {
            return await service.GetAsync(id);
        }
    }

    
}
