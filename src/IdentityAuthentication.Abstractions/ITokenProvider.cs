using Newtonsoft.Json.Linq;

namespace IdentityAuthentication.Abstractions
{
    public interface ITokenProvider
    {
        IToken GenerateToken(JObject values);

        Task<IToken> GenerateTokenAsync(JObject values);

        IToken RefreshToken(JObject values);

        Task<IToken> RefreshTokenAsync(JObject values);
    }
}
