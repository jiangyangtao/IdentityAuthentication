using IdentityAuthentication.Configuration;
using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Model.Configurations;
using IdentityAuthentication.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using IdentityAuthentication.Extensions;
using IdentityAuthentication.Core.Abstractions;

namespace IdentityAuthentication.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentityAuthentication(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            services.AddAuthenticationConfiguration(configuration);
            services.AddToken();

            services.AddSingleton<IAuthenticationHandler, AuthenticationHandler>();
            services.AddSingleton<IAuthenticationProvider, AuthenticationProvider>();
            services.AddSingleton<IAuthenticationConfigurationProvider, AuthenticationConfigurationProvider>();

            services.AddGrantSources(configuration);

            return services;
        }

        private static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<GrantDefaultConfiguration>(configuration.GetSection(GrantDefaultConfiguration.ConfigurationKey));
            services.Configure<AccessTokenConfiguration>(configuration.GetSection(AccessTokenConfiguration.ConfigurationKey));
            services.Configure<RefreshTokenConfiguration>(configuration.GetSection(RefreshTokenConfiguration.ConfigurationKey));
            services.Configure<AuthenticationConfiguration>(configuration.GetSection(AuthenticationConfiguration.ConfigurationKey));
            services.Configure<RsaSignatureConfiguration>(configuration.GetSection(RsaSignatureConfiguration.ConfigurationKey));
            services.Configure<SymmetricSignatureConfiguration>(configuration.GetSection(SymmetricSignatureConfiguration.ConfigurationKey));
            services.Configure<RsaEncryptionConfiguration>(configuration.GetSection(RsaEncryptionConfiguration.ConfigurationKey));
            services.Configure<AesEncryptionConfiguration>(configuration.GetSection(AesEncryptionConfiguration.ConfigurationKey));

            return services;
        }

        private static IServiceCollection AddGrantSources(this IServiceCollection services, IConfiguration configuration)
        {
            var authenticationInitializations = AuthenticationAssemblyBuilder.AuthenticationInitializations;
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
            var authenticationInitializations = AuthenticationAssemblyBuilder.AuthenticationInitializations;
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