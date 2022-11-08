using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Extensions;
using IdentityAuthentication.Model.Configurations;
using IdentityAuthentication.TokenServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace IdentityAuthentication.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentityAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddGrantSources(configuration);

            services.Configure<AccessTokenConfiguration>(configuration.GetSection("AccessToken"));
            services.Configure<RefreshTokenConfiguration>(configuration.GetSection("RefreshToken"));
            services.Configure<SecretKeyConfiguration>(configuration.GetSection("SecretKey"));
            services.Configure<AuthenticationConfiguration>(configuration.GetSection("Autnentication"));

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
                    if (authenticationInitialization != null) authenticationInitialization.InitializationService(services, configuration);
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
                    if (authenticationInitialization != null) authenticationInitialization.InitializationApplication(builder);
                }
            }

            return builder;
        }
    }
}