using Authentication.Abstractions;
using Authentication.Abstractions.Credentials;
using FirmAccount.Authentication.GrpcClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Firm
{
    public class FirmAuthenticationInitialization : IAuthenticationInitialization
    {
        public FirmAuthenticationInitialization()
        {
        }

        public IApplicationBuilder InitializationApplication(IApplicationBuilder builder) => builder;

        public IServiceCollection InitializationService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddFirmAccountAuthentication(options =>
            {
                options.Endpoint = configuration["Authentication:Firm:Endpoint"].ToString();
            });
            services.AddSingleton<IAuthenticationService<PasswordCredential>, FirmAuthenticationService>();
            return services;
        }
    }
}
