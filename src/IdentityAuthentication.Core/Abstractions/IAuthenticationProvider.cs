using IdentityAuthentication.Abstractions;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace IdentityAuthentication.Core.Abstractions
{
    public interface IAuthenticationProvider
    {
        Task<IToken> AuthenticateAsync(JObject credentialObject);

        Task<string> RefreshTokenAsync();

        Task<TokenValidationResult> ValidateTokenAsync(string token);

        Task<IReadOnlyDictionary<string, string>> GetTokenInfoAsync();
    }
}
