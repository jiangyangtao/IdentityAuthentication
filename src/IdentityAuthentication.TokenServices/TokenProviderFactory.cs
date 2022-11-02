using IdentityAuthenticaion.Model.Configurations;
using IdentityAuthentication.TokenServices.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAuthentication.TokenServices
{
    internal class TokenProviderFactory : ITokenProviderFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly AuthenticationConfiguration authenticationConfiguration;

        public TokenProviderFactory(IServiceProvider serviceProvider, IOptions<AuthenticationConfiguration> options)
        {
            _serviceProvider = serviceProvider;
            authenticationConfiguration = options.Value;
        }

        public ITokenProvider CreateTokenService()
        {
            return _serviceProvider.GetServices<ITokenProvider>().FirstOrDefault(a => a.TokenType == authenticationConfiguration.TokenType);
        }
    }
}
