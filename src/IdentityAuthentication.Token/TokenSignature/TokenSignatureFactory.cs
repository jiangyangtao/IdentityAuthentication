using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Token.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityAuthentication.Token.TokenSignature
{
    internal class TokenSignatureFactory : ITokenSignatureFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IAuthenticationConfigurationProvider _configurationProvider;

        public TokenSignatureFactory(IServiceProvider serviceProvider, IAuthenticationConfigurationProvider configurationProvider)
        {
            _serviceProvider = serviceProvider;
            _configurationProvider = configurationProvider;
        }

        public ITokenSignatureProvider CreateTokenSignatureProvider()
        {
            var provider = _serviceProvider.GetServices<ITokenSignatureProvider>().FirstOrDefault(a => a.TokenSignatureType == _configurationProvider.Authentication.TokenSignatureType);
            return provider ?? throw new Exception("Not found ITokenSignatureProvider the realize.");
        }
    }
}
