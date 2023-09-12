using Authentication.Abstractions;
using IdentityAuthentication.Model.Configurations;
using IdentityAuthentication.Abstractions;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace IdentityAuthentication.TokenServices.Abstractions
{
    public interface ITokenProvider
    {
        TokenType TokenType { get; }

        Task<IToken> GenerateAsync(AuthenticationResult authenticationResult);

        Task<string> RefreshAsync();

        Task<string> DestroyAsync();

        Task<AuthenticationResult> GetAuthenticationResultAsync();

        Task<TokenValidationResult> AuthorizeAsync(string token);

        Task<IReadOnlyDictionary<string, string>?> GetTokenInfoAsync();
    }
}
