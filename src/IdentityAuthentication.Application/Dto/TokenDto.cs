using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Model.Enums;

namespace IdentityAuthentication.Application.Dto
{
    public class TokenResultFactory : IDisposable
    {
        public IToken Token { get; }

        public TokenType TokenType { get; }

        public TokenResultFactory(IToken token, TokenType tokenType)
        {
            Token = token;
            TokenType = tokenType;
        }

        public object CreateTokenResult()
        {
            if (TokenType == TokenType.Reference)
            {
                return new AccessTokenResult(Token);
            }

            return new RefreshTokenResult(Token);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }

    public class AccessTokenBaseResult
    {
        public AccessTokenBaseResult(IToken token)
        {
            access_token = token.AccessToken;
        }

        public AccessTokenBaseResult(string accessToken)
        {
            access_token = accessToken;
        }

        public string access_token { set; get; }
    }

    public class AccessTokenResult : AccessTokenBaseResult
    {
        public AccessTokenResult(IToken token) : base(token)
        {
            expires_in = token.ExpiresIn;
            user_info = token.UserInfo;
        }

        public long expires_in { set; get; }

        public object user_info { set; get; }
    }

    public class RefreshTokenResult : AccessTokenResult
    {
        public RefreshTokenResult(IToken token) : base(token)
        {
            refresh_token = token.RefreshToken;
        }

        public string refresh_token { set; get; }
    }
}
