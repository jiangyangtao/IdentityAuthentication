﻿using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Model.Configurations;

namespace IdentityAuthentication.Application.Dto
{
    public class TokenResultFactory
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
            if (TokenType == TokenType.JWT)
            {
                return new TokenResult(Token);
            }

            return new RefreshTokenResult(Token);
        }
    }

    public class AccessTokenResult
    {
        public AccessTokenResult(string token)
        {
            access_token = token;
        }

        public string access_token { set; get; }
    }

    public class TokenResult : AccessTokenResult
    {
        public TokenResult(IToken token) : base(token.AccessToken)
        {
            expires_in = token.ExpiresIn;
            token_type = token.TokenType;
            user_info = token.UserInfo;
        }

        public long expires_in { set; get; }

        public string token_type { set; get; }

        public object user_info { set; get; }
    }

    public class RefreshTokenResult : TokenResult
    {
        public RefreshTokenResult(IToken token) : base(token)
        {
            refresh_token = token.RefreshToken;
        }

        public string refresh_token { set; get; }
    }
}
