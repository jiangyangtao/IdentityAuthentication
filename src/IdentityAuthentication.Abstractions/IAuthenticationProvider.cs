using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAuthentication.Abstractions
{
    public interface IAuthenticationProvider
    {
        Task<IToken> AuthenticateAsync(JObject credentialObject);

        IToken RefreshToken();

        Task<IToken> RefreshTokenAsync();
    }
}
