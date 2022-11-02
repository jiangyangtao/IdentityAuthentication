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
            services.AddMemoryCache(options =>
            {
                options.SizeLimit = 1024;
            });

            services.Configure<CacheStorageConfiguration>(configuration.GetSection("CacheStorage"));

            services.AddSingleton<ITokenProvider, JwtTokenProvider>();
            services.AddSingleton<ITokenProvider, ReferenceTokenProvider>();
            services.AddSingleton<ITokenProviderFactory, TokenProviderFactory>();

            services.AddSingleton<ICacheProvider, MemoryCacheProvider>();
            services.AddSingleton<ICacheProvider, RedisCacheProvider>();
            services.AddSingleton<ICacheProviderFactory, CacheProviderFactory>();
            return services;
        }
    }
}
