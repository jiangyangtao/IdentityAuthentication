using IdentityAuthentication.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAuthentication.Core
{
    internal class AuthenticationProvider : IAuthenticationProvider
    {
        public Task<IToken> Authenticate(object credential)
        {
            throw new NotImplementedException();
        }
    }
}
