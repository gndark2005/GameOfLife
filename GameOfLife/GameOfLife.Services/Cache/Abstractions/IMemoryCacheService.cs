namespace GameOfLife.Services.Cache.Abstractions
{
    public interface IMemoryCacheService
    {
        void AddToCache<T>(string key, T value);

        T GetFromCache<T>(string key);
    }
}
