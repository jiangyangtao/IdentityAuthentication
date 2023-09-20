using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Token.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityAuthentication.Token.TokenEncryption
{
    internal class TokenEncryptionProviderFactory : ITokenEncryptionProviderFactory
    {
        private readonly IAuthenticationConfigurationProvider _configurationProvider;
        private readonly IServiceProvider _serviceProvider;

        public TokenEncryptionProviderFactory(
            IAuthenticationConfigurationProvider configurationProvider,
            IServiceProvider serviceProvider)
        {
            _configurationProvider = configurationProvider;
            _serviceProvider = serviceProvider;
        }

        public ITokenEncryptionProvider CreateTokenEncryptionProvider()
        {
            return _serviceProvider.GetServices<ITokenEncryptionProvider>().FirstOrDefault(a => a.EncryptionType == _configurationProvider.Authentication.TokenEncryptionType);
        }
    }
}
