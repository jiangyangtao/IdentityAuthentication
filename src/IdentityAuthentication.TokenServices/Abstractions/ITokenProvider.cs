using Authentication.Abstractions;
using IdentityAuthentication.Model.Configurations;
using IdentityAuthentication.Abstractions;

namespace IdentityAuthentication.TokenServices.Abstractions
{
    public interface ITokenProvider
    {
        TokenType TokenType { get; }

        Task<IToken> GenerateAsync(AuthenticationResult authenticationResult);

        Task<string> RefreshAsync();

        Task<string> DestroyAsync();

        Task<AuthenticationResult?> GetAuthenticationResultAsync();

        Task<bool> AuthorizeAsync();

        Task<IReadOnlyDictionary<string, string>?> InfoAsync();
    }
}
