using Authentication.Abstractions;
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

        public IApplicationBuilder InitializationApplication(IApplicationBuilder builder)
        {
            throw new NotImplementedException();
        }

        public IServiceCollection InitializationService(IServiceCollection services, IConfiguration configuration)
        {
            throw new NotImplementedException();
        }
    }
}
