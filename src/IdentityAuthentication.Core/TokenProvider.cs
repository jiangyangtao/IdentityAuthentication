using IdentityAuthentication.Abstractions;
using Newtonsoft.Json.Linq;

namespace IdentityAuthentication.Core
{
    internal class TokenProvider : ITokenProvider
    {
        public TokenProvider()
        {
        }

        public IToken GenerateToken(JObject values)
        {
            throw new NotImplementedException();
        }

        public Task<IToken> GenerateTokenAsync(JObject values)
        {
            throw new NotImplementedException();
        }

        public IToken RefreshToken(JObject values)
        {
            throw new NotImplementedException();
        }

        public Task<IToken> RefreshTokenAsync(JObject values)
        {
            throw new NotImplementedException();
        }
    }
}
