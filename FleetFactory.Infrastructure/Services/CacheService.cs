using FleetFactory.Application.Interfaces.Services;
using Microsoft.Extensions.Caching.Memory;

namespace FleetFactory.Infrastructure.Services
{
    public class CacheService(IMemoryCache _cache) : ICacheService
    {
        public T? Get<T>(string key)
        {
            return _cache.TryGetValue(key, out T? value) ? value : default;
        }

        public void Set<T>(string key, T value, TimeSpan? expiration = null)
        {
            _cache.Set(key, value, expiration ?? TimeSpan.FromMinutes(30)); 
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

    }
}