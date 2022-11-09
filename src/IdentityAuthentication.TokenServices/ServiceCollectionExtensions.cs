using IdentityAuthentication.TokenServices.Abstractions;
using IdentityAuthentication.TokenServices.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityAuthentication.TokenServices
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTokenSerivces(this IServiceCollection services, IConfiguration configuration)
        {
            var cacheStorage = configuration.GetSection("CacheStorage");
            services.AddMemoryCache(options =>
            {
                var sizeLimit = cacheStorage.GetValue<int>("MemonySizeLimit");
                if (sizeLimit > 0) options.SizeLimit = sizeLimit;
            });

            services.Configure<CacheStorageConfiguration>(cacheStorage);

            services.AddSingleton<ITokenProvider, JwtTokenProvider>();
            services.AddSingleton<ITokenProvider, ReferenceTokenProvider>();
            services.AddSingleton<ITokenProviderFactory, TokenProviderFactory>();

            services.AddSingleton<ICacheProvider, MemoryCacheProvider>();
            services.AddSingleton<ICacheProvider, RedisCacheProvider>();
            services.AddSingleton<ICacheProviderFactory, CacheProviderFactory>();

            services.AddSingleton<RsaAlgorithmProvider>();

            return services;
        }
    }
}
