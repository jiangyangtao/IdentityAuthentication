using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Token.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityAuthentication.Token
{
    internal class TokenProviderFactory : ITokenProviderFactory
    {
        private readonly IAuthenticationConfigurationProvider _configurationProvider;
        private readonly IServiceProvider _serviceProvider;

        public TokenProviderFactory(
            IAuthenticationConfigurationProvider configurationProvider,
            IServiceProvider serviceProvider)
        {
            _configurationProvider = configurationProvider;
            _serviceProvider = serviceProvider;
        }

        public ITokenProvider CreateTokenService()
        {
            return _serviceProvider.GetServices<ITokenProvider>().FirstOrDefault(a => a.TokenType == _configurationProvider.Authentication.TokenType);
        }
    }
}
