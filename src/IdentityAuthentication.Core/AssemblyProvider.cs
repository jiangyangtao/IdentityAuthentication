using IdentityAuthentication.Extensions;
using Microsoft.Extensions.DependencyModel;
using System.Reflection;

namespace IdentityAuthentication.Core
{
    internal class AssemblyProvider
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
    }
}
