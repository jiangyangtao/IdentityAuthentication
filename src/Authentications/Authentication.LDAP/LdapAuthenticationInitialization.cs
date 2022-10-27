using Authentication.Abstractions;
using Authentication.Abstractions.Credentials;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.LDAP
{
    internal class LdapAuthenticationInitialization : IAuthenticationInitialization
    {
        public IApplicationBuilder InitializationApplication(IApplicationBuilder builder) => builder;

        public IServiceCollection InitializationService(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<LdapConfiguration>(configuration.GetSection("Authentication:LDAP"));
            services.AddSingleton<IAuthenticationService<PasswordCredential>, LdapAuthenticationService>();
            return services;
        }
    }
}
