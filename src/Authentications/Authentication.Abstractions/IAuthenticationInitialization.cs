using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Abstractions
{
    public interface IAuthenticationInitialization
    {
        public IServiceCollection Initialization(IServiceCollection services);
    }
}
