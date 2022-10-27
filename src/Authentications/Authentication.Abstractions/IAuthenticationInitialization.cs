using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Abstractions
{
    public interface IAuthenticationInitialization
    {
        public IServiceCollection InitializationService(IServiceCollection services, IConfiguration configuration);

        public IApplicationBuilder InitializationApplication(IApplicationBuilder builder);
    }
}
