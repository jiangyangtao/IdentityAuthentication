
using IdentityAuthentication.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityAuthentication.Authentication
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services)
        {
            services.AddSingleton<AuthenticationHandle>();
            services.AddSingleton<IAuthenticationProvider, AuthenticationProvider>();

            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            services.AddGrantSources(configuration);

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
