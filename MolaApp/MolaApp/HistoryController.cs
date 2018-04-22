using Akavache;
using MolaApp.Api;
using MolaApp.Model;
using MolaApp.Page;
using Newtonsoft.Json;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolaApp
{
    public class HistoryController
    {
        const int MAX_HISTORY_SIZE = 10;
        const string CACHE_KEY = "history";

        IBlobCache _cache;

        IDictionary<string, HistoryModel> _dict;

        public IList<HistoryModel> History
        {
            get
            {
                return _dict.Values.ToList();
            }
        }

        async public Task InitAsync()
        {
            _cache = BlobCache.UserAccount;
            _dict = await _cache.GetOrCreateObject<Dictionary<string, HistoryModel>>(CACHE_KEY, () => new Dictionary<string, HistoryModel>());
        }

        public async Task SetScannedNow(string profileId)
        {
            HistoryModel model = null;
            if(!_dict.TryGetValue(profileId, out model))
            {
                model = new HistoryModel();
                model.ProfileId = profileId;

                if(_dict.Count >= MAX_HISTORY_SIZE)
                {
                    // Delete oldest to free space for the new element
                    List<HistoryModel> list = _dict.Values.ToList();
                    list.Sort((x, y) => DateTimeOffset.Compare(x.Date, y.Date));
                    _dict.Remove(list.Last().ProfileId);
                }

                _dict.Add(profileId, model);
            }
            model.Date = DateTimeOffset.Now;
            
            await _cache.InsertObject(CACHE_KEY, _dict);
        }
    }
}
