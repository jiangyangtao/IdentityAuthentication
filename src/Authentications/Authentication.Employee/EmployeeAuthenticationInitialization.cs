using Authentication.Abstractions.Credentials;
using Authentication.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Employee.GrpcClient;

namespace Authentication.Employee
{
    public class EmployeeAuthenticationInitialization : IAuthenticationInitialization
    {
        public IApplicationBuilder InitializationApplication(IApplicationBuilder builder) => builder;

        public IServiceCollection InitializationService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddEmpoyeeGrpcClient(options =>
            {
                options.Endpoint = configuration["Authentication:Employee:Endpoint"].ToString();
            });
            services.AddSingleton<IAuthenticationService<PasswordCredential>, EmployeeAuthenticationService>();
            return services;
        }
    }
}