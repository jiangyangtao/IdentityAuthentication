using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Extensions;
using IdentityAuthentication.TokenServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityAuthentication.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentityAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddGrantSources(configuration);

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