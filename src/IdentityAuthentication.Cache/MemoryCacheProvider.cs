using IdentityAuthentication.TokenServices.Abstractions;
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

        public StorageType StorageType => StorageType.Memory;

        public Task<ReferenceToken> GetAsync(string key)
        {
            var data = _memoryCache.Get<ReferenceToken>(key);
            if (data == null) return null;

            return Task.FromResult(data);
        }

        public Task RemoveAsync(string key)
        {
            _memoryCache.Remove(key);
            return Task.CompletedTask;
        }

        public Task SetAsync(string key, ReferenceToken data)
        {
            _memoryCache.Set(key, data, DateTimeOffset.Now.AddDays(_cacheStorageConfiguration.CacheExpirationTime));
            return Task.CompletedTask;
        }
    }
}
