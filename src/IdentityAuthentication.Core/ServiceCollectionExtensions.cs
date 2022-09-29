using Authentication.Abstractions;
using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System.Reflection;

namespace IdentityAuthentication.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentityAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication();
            services.AddSingleton<ITokenProvider, TokenProvider>();
            services.AddSingleton<IAuthenticationProvider, AuthenticationProvider>();

            return services;
        }

        private static IServiceCollection AddAuthentication(this IServiceCollection services)
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