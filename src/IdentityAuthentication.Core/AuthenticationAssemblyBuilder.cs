using Authentication.Abstractions;
using IdentityAuthentication.Extensions;
using Microsoft.Extensions.DependencyModel;
using System.Reflection;

namespace IdentityAuthentication.Core
{
    internal class AuthenticationAssemblyBuilder
    {
        private static IReadOnlyCollection<Assembly> AuthenticationAssemblies;

        public static IReadOnlyCollection<Assembly> Authentications
        {
            get
            {
                if (AuthenticationAssemblies.IsNullOrEmpty())
                {
                    var compilationLibraries = DependencyContext.Default.GetCompileLibraries("Authentication");
                    if (compilationLibraries.IsNullOrEmpty()) return Array.Empty<Assembly>();

                    var assemblys = new List<Assembly>();
                    foreach (var compilationLibrary in compilationLibraries)
                    {
                        var assemblie = Assembly.Load(compilationLibrary.Name);
                        assemblys.Add(assemblie);
                    }

                    AuthenticationAssemblies = assemblys;
                }

                return AuthenticationAssemblies;
            }
        }

        private static IReadOnlyCollection<IAuthenticationInitialization> AuthenticationInitializationInterfaces;

        public static IReadOnlyCollection<IAuthenticationInitialization> AuthenticationInitializations
        {
            get
            {
                if (AuthenticationInitializationInterfaces.IsNullOrEmpty())
                {
                    if (Authentications.IsNullOrEmpty()) return Array.Empty<IAuthenticationInitialization>();

                    var authenticationsAssemblies = Authentications.Where(a => a.FullName.Contains("Abstractions") == false).ToArray();
                    if (authenticationsAssemblies.IsNullOrEmpty()) return Array.Empty<IAuthenticationInitialization>();

                    var authenticationInitializations = new List<IAuthenticationInitialization>();
                    foreach (var assemblie in authenticationsAssemblies)
                    {
                        var types = assemblie.GetTypes();
                        if (types.IsNullOrEmpty()) continue;

                        var interfaceName = nameof(IAuthenticationInitialization);
                        foreach (var type in types)
                        {
                            var interfaceType = type.GetInterface(interfaceName, true);
                            if (interfaceType != null)
                            {
                                var instance = (IAuthenticationInitialization)Activator.CreateInstance(type);
                                if (instance != null) authenticationInitializations.Add(instance);
                            }
                        }
                    }

                    AuthenticationInitializationInterfaces = authenticationInitializations;
                }

                return AuthenticationInitializationInterfaces;
            }
        }
    }
}
