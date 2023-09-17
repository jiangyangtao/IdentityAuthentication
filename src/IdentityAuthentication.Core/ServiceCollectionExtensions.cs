using IdentityAuthentication.Configuration;
using IdentityAuthentication.Configuration.Abstractions;
using IdentityAuthentication.Configuration.Enums;
using IdentityAuthentication.Model.Configurations;
using IdentityAuthentication.Model.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityAuthentication.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentityAuthentication(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddAuthenticationConfiguration(); ;

            services.AddSingleton<IAuthenticationHandler, AuthenticationHandldr>();
            services.AddSingleton<IAuthenticationConfigurationBuilder, AuthenticationConfigurationBuilder>();
            return services;
        }

        private static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            services.Configure<GrantDefaultConfiguration>(configuration.GetSection(GrantDefaultConfiguration.ConfigurationKey));
            services.Configure<AccessTokenConfiguration>(configuration.GetSection(AccessTokenConfiguration.ConfigurationKey));
            services.Configure<RefreshTokenConfiguration>(configuration.GetSection(RefreshTokenConfiguration.ConfigurationKey));

            var authenticationConfig = configuration.GetSection(AuthenticationConfiguration.ConfigurationKey);
            services.Configure<AuthenticationConfiguration>(authenticationConfig);

            var tokenType = authenticationConfig.GetValue<TokenType>(nameof(AuthenticationConfiguration.TokenType));
            var tokenSignatureType = authenticationConfig.GetValue<TokenSignatureType>(nameof(AuthenticationConfiguration.TokenSignatureType));
            var tokenEncryptionType = authenticationConfig.GetValue<TokenEncryptionType>(nameof(AuthenticationConfiguration.TokenEncryptionType));
            if (tokenType == TokenType.JWT || tokenType == TokenType.Reference)
            {
                if (tokenSignatureType == TokenSignatureType.Rsa)
                {
                    services.Configure<RsaSignatureConfiguration>(configuration.GetSection(RsaSignatureConfiguration.ConfigurationKey));
                }

                if (tokenSignatureType == TokenSignatureType.Symmetric)
                {
                    services.Configure<SymmetricSignatureConfiguration>(configuration.GetSection(SymmetricSignatureConfiguration.ConfigurationKey));
                }
            }

            if (tokenType == TokenType.Encrypt)
            {
                if (tokenEncryptionType == TokenEncryptionType.Rsa)
                {
                    services.Configure<RsaEncryptionConfiguration>(configuration.GetSection(RsaEncryptionConfiguration.ConfigurationKey));
                }

                if (tokenEncryptionType == TokenEncryptionType.Aes)
                {
                    services.Configure<AesEncryptionConfiguration>(configuration.GetSection(AesEncryptionConfiguration.ConfigurationKey));
                }
            }

            return services;
        }
    }
}