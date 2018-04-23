using MolaApp.Model;
using System.Threading.Tasks;

namespace MolaApp.Api
{
    public interface IImageApi : IGetObjectApi<ImageModel>
    {
        Task<bool> PutAsync(ImageModel image);
    }
}
