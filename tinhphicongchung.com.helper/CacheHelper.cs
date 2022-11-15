using System;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace tinhphicongchung.com.helper
{
    public class CacheHelper
    {
        private static ObjectCache _cache = MemoryCache.Default;
        private CacheItemPolicy _policy = null;
        private CacheEntryRemovedCallback _removedCallback = null;

        public T Get<T>(string cacheKeyName)
        {
            return (T) _cache[cacheKeyName];
        }

        public void Remove(string cacheKeyName)
        {
            if (_cache.Contains(cacheKeyName))
            {
                _cache.Remove(cacheKeyName);
            }
        }

        public void Set<T>(string cacheKeyName, T cacheItem, int timeInSecond = 120, CachePriority cachePriority = CachePriority.Default)
        {
            _removedCallback = new CacheEntryRemovedCallback(this.CachedItemRemovedCallback);
            
            _policy = new CacheItemPolicy
            {
                Priority = (cachePriority == CachePriority.Default) ? CacheItemPriority.Default : CacheItemPriority.NotRemovable,
                AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(timeInSecond),
                RemovedCallback = _removedCallback
            };

            // Add inside cache 
            _cache.Set(cacheKeyName, cacheItem, _policy);
        }

        public async Task<T> GetOrSet<T>(string cacheKeyName, Func<Task<T>> getDataFromDbCallback, int timeInSecond = 120, CachePriority cachePriority = CachePriority.Default)
        {
            var resultVar = Get<T>(cacheKeyName);

            if (resultVar == null)
            {
                resultVar = await getDataFromDbCallback();

                if(resultVar != null)
                {
                    _removedCallback = new CacheEntryRemovedCallback(this.CachedItemRemovedCallback);

                    _policy = new CacheItemPolicy
                    {
                        Priority = (cachePriority == CachePriority.Default) ? CacheItemPriority.Default : CacheItemPriority.NotRemovable,
                        AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(timeInSecond),
                        RemovedCallback = _removedCallback
                    };

                    // Add inside cache 
                    _cache.Set(cacheKeyName, resultVar, _policy);
                }
            }

            return resultVar;
        }

        private void CachedItemRemovedCallback(CacheEntryRemovedArguments arguments)
        {
            LogHelper.Info(string.Concat("Reason: ", arguments.RemovedReason.ToString(), " | Key - Name: ", arguments.CacheItem.Key, " | Value - Object: ", arguments.CacheItem.Value.ToString()));
        }
    }

    public enum CachePriority
    {
        Default,
        NotRemovable
    }
}
