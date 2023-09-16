using IdentityAuthentication.Token.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAuthentication.Token
{
    internal class TokenProviderFactory : ITokenProviderFactory
    {
        public TokenProviderFactory()
        {
        }

        public ITokenProvider CreateTokenService()
        {
            throw new NotImplementedException();
        }
    }
}
