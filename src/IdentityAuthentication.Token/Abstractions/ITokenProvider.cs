using IdentityAuthentication.Configuration.Abstractions;
using IdentityAuthentication.Model.Enums;
using Microsoft.IdentityModel.Tokens;

namespace IdentityAuthentication.Token.Abstractions
{
    public interface ITokenProvider
    {
        TokenType TokenType { get; }

        Task<IToken> GenerateAsync(IAuthenticationResult authenticationResult);

        Task<string> RefreshAsync();

        Task<string> DestroyAsync();

        Task<IAuthenticationResult> GetAuthenticationResultAsync();

        Task<TokenValidationResult> AuthorizeAsync();

        Task<IReadOnlyDictionary<string, string>?> GetTokenInfoAsync();
    }
}
