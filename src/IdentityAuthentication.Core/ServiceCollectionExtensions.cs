using IdentityAuthentication.Configuration;
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

            var tokenType = authenticationConfig.GetValue<TokenType>(nameof(TokenType));
            if (tokenType == TokenType.JWT || tokenType == TokenType.Reference)
            {
                var signatureConfig = configuration.GetSection(TokenSignatureConfiguration.ConfigurationKey);
                var algorithmType = signatureConfig.GetValue<TokenSignatureAlgorithmType>(nameof(TokenSignatureConfiguration.AlgorithmType));           
                services.Configure<TokenSignatureConfiguration>(a =>
                {
                    a.AlgorithmType = algorithmType;

                    var rsaSignatureConfig = signatureConfig.GetSection(RsaSignatureConfiguration.ConfigurationKey);
                    a.RsaSignature = new RsaSignatureConfiguration
                    {
                        AlgorithmType = algorithmType;
                    };
                });
            }

            if (tokenType == TokenType.Encrypt)
            {
                services.Configure<TokenSignatureConfiguration>(configuration.GetSection(TokenSignatureConfiguration.ConfigurationKey));
            }

            return services;
        }
    }
}