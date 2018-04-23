using Akavache;
using MolaApp.Model;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace MolaApp.Api
{
    abstract public class AbstractApi
    {
        protected HttpClient client;

        public AbstractApi(HttpClient httpClient)
        {
            client = httpClient;
        }

        protected StringContent CreateJsonContent(object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            return content;
        }
    }
}
