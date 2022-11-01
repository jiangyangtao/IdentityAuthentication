using IdentityAuthentication.TokenServices.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityAuthentication.TokenServices
{
    internal class CacheProviderFactory : ICacheProviderFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public CacheProviderFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ICacheProvider CreateCacheProvider(StorageType storageType)
        {
            return _serviceProvider.GetServices<ICacheProvider>().FirstOrDefault(a => a.StorageType == storageType);
        }
    }
}
