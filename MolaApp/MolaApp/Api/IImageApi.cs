using MolaApp.Model;
using System.Threading.Tasks;

namespace MolaApp.Api
{
    public interface IImageApi : IApi<ImageModel>
    {
        Task<bool> PutAsync(ImageModel image);
    }
}
