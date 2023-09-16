using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Extensions;
using IdentityAuthentication.Model.Configurations;
using IdentityAuthentication.Model.Models;
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
            services.Configure<GrantDefaults>(configuration.GetSection("GrantDefaults"));
            services.Configure<AccessTokenConfiguration>(configuration.GetSection("AccessToken"));
            services.Configure<TokenConfigurationBase>(configuration.GetSection("RefreshToken"));
            services.Configure<SecretKeyConfiguration>(configuration.GetSection("SecretKey"));
            services.Configure<AuthenticationConfiguration>(configuration.GetSection("Authentication"));

            services.AddSingleton<AuthenticationHandle>();
            services.AddSingleton<IAuthenticationProvider, AuthenticationProvider>();

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