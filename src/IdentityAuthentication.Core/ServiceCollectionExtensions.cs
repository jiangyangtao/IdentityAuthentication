﻿using Authentication.Abstractions;
using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityAuthentication.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentityAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddAuthenticationSources();

            services.AddSingleton<ClaimService>();
            services.AddSingleton<TokenProvider>();
            services.AddSingleton<IAuthenticationProvider, AuthenticationProvider>();
            services.Configure<AuthenticationConfig>(configuration.GetSection("Autnentication"));

            return services;
        }

        private static IServiceCollection AddAuthenticationSources(this IServiceCollection services)
        {
            var assemblies = AssemblyProvider.Authentications;
            if (assemblies.NotNullAndEmpty())
            {
                var authenticationsAssemblies = assemblies.Where(a => a.FullName.Contains("Abstractions") == false).ToArray();
                foreach (var assemblie in authenticationsAssemblies)
                {
                    var types = assemblie.GetTypes();
                    foreach (var type in types)
                    {
                        var interfaceType = type.GetInterface(nameof(IAuthenticationInitialization), true);
                        if (interfaceType != null)
                        {
                            var authenticationInitialization = (IAuthenticationInitialization)Activator.CreateInstance(type);
                            if (authenticationInitialization != null) authenticationInitialization.Initialization(services);
                        }
                    }
                }
            }

            return services;
        }
    }
}