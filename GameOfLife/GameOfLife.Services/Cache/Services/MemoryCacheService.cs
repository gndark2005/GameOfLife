using GameOfLife.Services.Cache.Abstractions;
using Microsoft.Extensions.Caching.Memory;

namespace GameOfLife.Services.Cache.Services
{
    public class MemoryCacheService : IMemoryCacheService
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void AddToCache<T>(string key, T value)
        {
            _memoryCache.Set(key, value, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5)));
        }

        public T GetFromCache<T>(string key)
        {
            return _memoryCache.Get<T>(key)!;
        }
    }
}
