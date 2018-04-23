using MolaApp.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MolaApp.Api
{
    public interface IUserApi : IGetObjectApi<UserModel>
    {
        Task CreateAsync(UserModel model);

        Task<AuthToken> GetTokenAsync(UserModel credentials);
    }
}
