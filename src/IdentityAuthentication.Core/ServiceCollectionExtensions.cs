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

            services.AddGrantSources(configuration);
            services.Configure<GrantDefaultConfiguration>(configuration.GetSection("GrantDefaults"));
            services.Configure<AccessTokenConfiguration>(configuration.GetSection("AccessToken"));
            services.Configure<RefreshTokenConfiguration>(configuration.GetSection("RefreshToken"));
            services.Configure<AuthenticationConfiguration>(configuration.GetSection("Authentication"));

            services.AddSingleton<AuthenticationHandle>();
            services.AddSingleton<IAuthenticationHandler, AuthenticationHandldr>();

            services.AddTokenSerivces(configuration);

            return services;
        }

        private static IServiceCollection AddGrantSources(this IServiceCollection services, IConfiguration configuration)
        {
            var authenticationInitializations = AssemblyProvider.AuthenticationInitializations;
            if (authenticationInitializations.NotNullAndEmpty())
            {
                foreach (var authenticationInitialization in authenticationInitializations)
                {
                    authenticationInitialization?.InitializationService(services, configuration);
                }
            }

            return services;
        }

        public static IApplicationBuilder UseIdentityAuthentication(this IApplicationBuilder builder)
        {
            var authenticationInitializations = AssemblyProvider.AuthenticationInitializations;
            if (authenticationInitializations.NotNullAndEmpty())
            {
                foreach (var authenticationInitialization in authenticationInitializations)
                {
                    authenticationInitialization?.InitializationApplication(builder);
                }
            }

            return builder;
        }
    }
}