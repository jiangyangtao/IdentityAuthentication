using IdentityAuthentication.Cache.Abstractions;
using IdentityAuthentication.Configuration;
using IdentityAuthentication.Configuration.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace IdentityAuthentication.Cache
{
    internal class CacheProviderFactory<T> : ICacheProviderFactory<T> where T : IAuthenticationResult, new()
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly CacheStorageConfiguration _cacheStorageConfiguration;

        public CacheProviderFactory(IServiceProvider serviceProvider, IOptions<CacheStorageConfiguration> options)
        {
            _serviceProvider = serviceProvider;
            _cacheStorageConfiguration = options.Value;
        }

        public ICacheProvider<T> CreateCacheProvider()
        {
            return _serviceProvider.GetServices<ICacheProvider<T>>().FirstOrDefault(a => a.StorageType == _cacheStorageConfiguration.StorageType);
        }
    }
}
