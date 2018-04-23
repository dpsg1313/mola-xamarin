using MolaApp.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MolaApp.Api
{
    public interface IGetObjectApi<T> where T : IModel
    {
        Task<T> GetAsync(string id);

        IObservable<T> Get(string id);
    }
}
