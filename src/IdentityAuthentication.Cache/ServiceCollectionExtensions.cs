using IdentityAuthentication.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityAuthentication.Cache
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCacheStorage(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            services.Configure<CacheStorageConfiguration>(configuration.GetSection("CacheStorage"));

            var cacheStorage = configuration.GetSection("CacheStorage");
            services.AddMemoryCache(options =>
            {
                var sizeLimit = cacheStorage.GetValue<int>("MemonySizeLimit");
                if (sizeLimit > 0) options.SizeLimit = sizeLimit;
            });

            services.AddSingleton<ICacheProvider, MemoryCacheProvider>();
            services.AddSingleton<ICacheProvider, RedisCacheProvider>();
            services.AddSingleton<ICacheProviderFactory, CacheProviderFactory>();

            return services;
        }
    }
}
