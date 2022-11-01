using IdentityAuthenticaion.Model.Configurations;
using IdentityAuthentication.TokenServices.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAuthentication.TokenServices
{
    internal class TokenServiceFactory : ITokenServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public TokenServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ITokenService CreateTokenService(TokenType tokenType)
        {
            return _serviceProvider.GetServices<ITokenService>().FirstOrDefault(a => a.TokenType == tokenType);
        }
    }
}
