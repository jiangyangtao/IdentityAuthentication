using IdentityAuthentication.Abstractions;

namespace IdentityAuthentication.Application.Dto
{
    public class TokenDtoBase
    {
        public TokenDtoBase(string token)
        {
            access_token = token;
        }

        public string access_token { set; get; }
    }

    public class TokenDto : TokenDtoBase
    {
        public TokenDto(IToken token) : base(token.AccessToken)
        {
            expires_in = token.ExpiresIn;
            token_type = token.TokenType;
            refresh_token = token.RefreshToken;
        }

        public long expires_in { set; get; }

        public string token_type { set; get; }

        public string refresh_token { set; get; }
    }
}
