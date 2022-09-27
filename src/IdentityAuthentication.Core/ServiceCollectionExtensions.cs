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
            var compilationLibraries = DependencyContext.Default.GetCompileLibraries("Authentication");
            if (compilationLibraries.NotNullAndEmpty())
            {
                var authenticationCompilationLibraries = compilationLibraries.Where(a => a.Name.Contains("Abstractions") == false).ToArray();
                foreach (var compilationLibrary in authenticationCompilationLibraries)
                {
                    var assemblie = Assembly.Load(compilationLibrary.Name);
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