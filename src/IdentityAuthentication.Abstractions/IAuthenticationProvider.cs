using Newtonsoft.Json.Linq;

namespace IdentityAuthentication.Abstractions
{
    public interface IAuthenticationProvider
    {
        Task<IToken> AuthenticateAsync(JObject credentialObject);

        Task<string> RefreshTokenAsync();

        Task<bool> AuthorizeAsync();

        Task<JObject> TokenInfoAsync();
    }
}
