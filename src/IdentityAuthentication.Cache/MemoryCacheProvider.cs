using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Cache.Abstractions;
using IdentityAuthentication.Configuration;
using IdentityAuthentication.Configuration.Enums;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace IdentityAuthentication.Cache
{
    internal class MemoryCacheProvider : ICacheProvider
    {
        private readonly IMemoryCache _memoryCache;
        private readonly CacheStorageConfiguration _cacheStorageConfiguration;

        public MemoryCacheProvider(IMemoryCache memoryCache, IOptions<CacheStorageConfiguration> options)
        {
            _memoryCache = memoryCache;
            _cacheStorageConfiguration = options.Value;
        }

        public CacheStorageType StorageType => CacheStorageType.Memory;

        public Task<T?> GetAsync<T>(string key) where T : IAuthenticationResult, new()
        {
            var data = _memoryCache.Get<T>(key);
            if (data == null) return null;

            return Task.FromResult(data);
        }

        public Task RemoveAsync(string key)
        {
            _memoryCache.Remove(key);
            return Task.CompletedTask;
        }

        public Task SetAsync<T>(string key, T data) where T : IAuthenticationResult, new()
        {
            _memoryCache.Set(key, data, DateTimeOffset.Now.AddDays(_cacheStorageConfiguration.CacheExpirationTime));
            return Task.CompletedTask;
        }
    }
}
