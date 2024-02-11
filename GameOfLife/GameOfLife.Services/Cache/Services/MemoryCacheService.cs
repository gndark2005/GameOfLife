using GameOfLife.Services.Boards.Services;
using GameOfLife.Services.Cache.Abstractions;
using GameOfLife.Services.Cache.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GameOfLife.Services.Cache.Services
{
    public class MemoryCacheService : IMemoryCacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<BoardService> _logger;
        private readonly MemoryCacheSettings _memoryCacheSettings;

        public MemoryCacheService(
            IMemoryCache memoryCache,
            ILogger<BoardService> logger,
            IOptions<MemoryCacheSettings> memoryCacheSettings)
        {
            _memoryCache = memoryCache;
            _logger = logger;
            _memoryCacheSettings = memoryCacheSettings.Value;
        }

        public void AddToCache<T>(string key, T value)
        {
            var slidingExpiration = TimeSpan.FromSeconds(_memoryCacheSettings.SlidingExpirationInSeconds);
            _memoryCache.Set(key, value, new MemoryCacheEntryOptions().SetSlidingExpiration(slidingExpiration));
        }

        public T GetFromCache<T>(string key)
        {
            var result = _memoryCache.Get<T>(key)!;

            if (result is null)
            {
                _logger.LogDebug("Cache miss for key: {key}", key);
            }

            return result;
        }
    }
}
