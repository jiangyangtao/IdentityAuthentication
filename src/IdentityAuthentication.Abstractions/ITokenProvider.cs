using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace IdentityAuthentication.Abstractions
{
    public interface ITokenProvider
    {
        IToken GenerateToken(Claim[] claims);

        Task<IToken> GenerateTokenAsync(string id, JObject values);

        IToken RefreshToken();

        Task<IToken> RefreshTokenAsync();
    }
}
