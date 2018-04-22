using Akavache;
using MolaApp.Api;
using MolaApp.Model;
using Newtonsoft.Json;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolaApp
{
    public class BookmarkController
    {
        const string CACHE_KEY = "bookmarks";

        IBlobCache _cache;

        public ISet<string> ProfileIds { get; private set; }

        async public Task InitAsync()
        {
            _cache = BlobCache.UserAccount;
            ProfileIds = await _cache.GetOrCreateObject<HashSet<string>>(CACHE_KEY, () => new HashSet<string>());
        }

        public async Task AddBookmark(string profileId)
        {
            ProfileIds.Add(profileId);
            await _cache.InsertObject(CACHE_KEY, ProfileIds);
        }

        public async Task RemoveBookmark(string profileId)
        {
            ProfileIds.Remove(profileId);
            await _cache.InsertObject(CACHE_KEY, ProfileIds);
        }

        public bool IsBookmarked(string profileId)
        {
            return ProfileIds.Contains(profileId);
        }
    }
}
