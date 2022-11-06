using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace IdentityAuthentication.Abstractions
{
    public interface IAuthenticationProvider
    {
        Task<IToken> AuthenticateAsync(JObject credentialObject);

        Task<string> RefreshTokenAsync();

        Task<TokenValidationResult> AuthorizeAsync(string token);

        Task<JObject> TokenInfoAsync();
    }
}
