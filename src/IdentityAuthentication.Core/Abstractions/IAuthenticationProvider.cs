using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace IdentityAuthentication.Core
{
    public interface IAuthenticationProvider
    {
        Task<ITokenResult> AuthenticateAsync(JObject credentialObject);

        Task<string> RefreshTokenAsync();

        Task<TokenValidationResult> ValidateTokenAsync(string token);

        Task<IReadOnlyDictionary<string, string>> GetTokenInfoAsync();
    }
}
