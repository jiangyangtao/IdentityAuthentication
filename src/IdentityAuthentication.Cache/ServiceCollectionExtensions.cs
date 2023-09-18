using IdentityAuthentication.Cache.Abstractions;
using IdentityAuthentication.Configuration;
using IdentityAuthentication.Model.Enums;
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

            var authenticationConfig = configuration.GetSection(AuthenticationConfiguration.ConfigurationKey);
            var tokenType = authenticationConfig.GetValue<TokenType>(nameof(AuthenticationConfiguration.TokenType));
            if (tokenType != TokenType.Reference) return services;

            var cacheStorage = configuration.GetSection(CacheStorageConfiguration.ConfigurationKey);
            services.Configure<CacheStorageConfiguration>(cacheStorage);

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
