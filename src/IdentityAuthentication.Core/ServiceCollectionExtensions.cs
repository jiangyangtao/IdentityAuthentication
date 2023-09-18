﻿using IdentityAuthentication.Configuration;
using IdentityAuthentication.Configuration.Abstractions;
using IdentityAuthentication.Model.Configurations;
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
            services.AddSingleton<IAuthenticationConfigurationProvider, AuthenticationConfigurationProvider>();
            return services;
        }

        private static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

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
    }
}