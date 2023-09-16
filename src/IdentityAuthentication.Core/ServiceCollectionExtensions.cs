using IdentityAuthentication.Configuration;
using IdentityAuthentication.Extensions;
using IdentityAuthentication.Model.Configurations;
using IdentityAuthentication.TokenServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityAuthentication.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentityAuthentication(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            services.Configure<GrantDefaultConfiguration>(configuration.GetSection("GrantDefaults"));
            services.Configure<AccessTokenConfiguration>(configuration.GetSection("AccessToken"));
            services.Configure<RefreshTokenConfiguration>(configuration.GetSection("RefreshToken"));
            services.Configure<AuthenticationConfiguration>(configuration.GetSection("Authentication"));

            services.AddSingleton<IAuthenticationHandler, AuthenticationHandldr>();
            return services;
        }
    }
}