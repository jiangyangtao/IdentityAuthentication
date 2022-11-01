using IdentityAuthentication.TokenServices.Abstractions;
using IdentityAuthentication.TokenServices.Providers;
using IdentityAuthentication.TokenServices.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityAuthentication.TokenServices
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTokenSerivces(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CacheStorageConfiguration>(configuration.GetSection("CacheStorage"));

            services.AddSingleton<ITokenService, JwtTokenService>();
            services.AddSingleton<ITokenService, ReferenceTokenService>();
            services.AddSingleton<ITokenServiceFactory, TokenServiceFactory>();
 
            services.AddSingleton<ICacheProvider, MemoryProvider>();
            services.AddSingleton<ICacheProvider, RedisProvider>();
            services.AddSingleton<ICacheProviderFactory, CacheProviderFactory>();
            return services;
        }
    }
}
