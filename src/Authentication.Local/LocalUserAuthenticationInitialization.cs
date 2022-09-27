using Authentication.Abstractions;
using Authentication.Abstractions.Credentials;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Local
{
    internal class LocalUserAuthenticationInitialization : IAuthenticationInitialization
    {
        public IServiceCollection Initialization(IServiceCollection services)
        {
            services.AddSingleton<IAuthenticationService<PasswordCredential>, LocalUserAuthenticationService>();
            return services;
        }
    }
}
