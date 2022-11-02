using IdentityAuthentication.TokenServices.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace IdentityAuthentication.TokenServices
{
    internal class CacheProviderFactory : ICacheProviderFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly CacheStorageConfiguration _cacheStorageConfiguration;

        public CacheProviderFactory(IServiceProvider serviceProvider, IOptions<CacheStorageConfiguration> options)
        {
            _serviceProvider = serviceProvider;
            _cacheStorageConfiguration = options.Value;
        }

        public ICacheProvider CreateCacheProvider()
        {
            return _serviceProvider.GetServices<ICacheProvider>().FirstOrDefault(a => a.StorageType == _cacheStorageConfiguration.StorageType);
        }
    }
}
