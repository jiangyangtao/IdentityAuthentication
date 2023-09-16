using IdentityAuthentication.Configuration.Model;
using IdentityAuthentication.Model.Enums;
using Microsoft.IdentityModel.Tokens;

namespace IdentityAuthentication.Token.Abstractions
{
    public interface ITokenProvider
    {
        TokenType TokenType { get; }

        Task<ITokenResult> GenerateAsync(AuthenticationResult authenticationResult);

        Task<string> RefreshAsync();

        Task<string> DestroyAsync();

        Task<AuthenticationResult> GetAuthenticationResultAsync();

        Task<TokenValidationResult> AuthorizeAsync(string token);

        Task<IReadOnlyDictionary<string, string>?> GetTokenInfoAsync();
    }
}
