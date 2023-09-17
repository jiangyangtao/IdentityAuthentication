using IdentityAuthentication.Configuration.Abstractions;
using IdentityAuthentication.Token.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAuthentication.Token
{
    internal class TokenValidationProvider : ITokenValidationProvider
    {
        private readonly IAuthenticationConfigurationProvider _configurationProvider;

        public TokenValidationProvider(IAuthenticationConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }
    }
}
