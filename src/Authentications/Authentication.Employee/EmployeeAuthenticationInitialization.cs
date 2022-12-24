using Authentication.Abstractions.Credentials;
using Authentication.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Employee
{
    public class EmployeeAuthenticationInitialization : IAuthenticationInitialization
    {
        public IApplicationBuilder InitializationApplication(IApplicationBuilder builder) => builder;

        public IServiceCollection InitializationService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IAuthenticationService<PasswordCredential>, EmployeeAuthenticationService>();
            return services;
        }
    }
}