using Newtonsoft.Json.Linq;

namespace IdentityAuthentication.Abstractions
{
    public interface ITokenProvider
    {
        IToken GenerateToken(string id, JObject values);

        Task<IToken> GenerateTokenAsync(string id, JObject values);

        IToken RefreshToken();

        Task<IToken> RefreshTokenAsync();
    }
}
