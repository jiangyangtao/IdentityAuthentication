﻿using IdentityAuthenticaion.Model.Configurations;
using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Extensions;
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
            services.AddAuthenticationSources(configuration);

            services.AddSingleton<TokenProvider>();
            services.AddSingleton<AuthenticationHandle>();
            services.AddSingleton<IAuthenticationProvider, AuthenticationProvider>();

            services.Configure<AccessTokenConfiguration>(configuration.GetSection("AccessToken"));
            services.Configure<RefreshTokenConfiguration>(configuration.GetSection("RefreshToken"));
            services.Configure<SecretKeyConfiguration>(configuration.GetSection("SecretKey"));
            services.Configure<AuthenticationConfiguration>(configuration.GetSection("Autnentication"));

            return services;
        }

        private static IServiceCollection AddAuthenticationSources(this IServiceCollection services, IConfiguration configuration)
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