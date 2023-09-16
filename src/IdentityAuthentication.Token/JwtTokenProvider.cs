﻿using IdentityAuthentication.Configuration.Model;
using IdentityAuthentication.Model.Enums;
using IdentityAuthentication.Token.Abstractions;
using Microsoft.IdentityModel.Tokens;

namespace IdentityAuthentication.Token
{
    public class JwtTokenProvider : ITokenProvider
    {
        public JwtTokenProvider()
        {
        }

        public TokenType TokenType => throw new NotImplementedException();

        public Task<TokenValidationResult> AuthorizeAsync(string token)
        {
            throw new NotImplementedException();
        }

        public Task<string> DestroyAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ITokenResult> GenerateAsync(AuthenticationResult authenticationResult)
        {
            throw new NotImplementedException();
        }

        public Task<AuthenticationResult> GetAuthenticationResultAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyDictionary<string, string>?> GetTokenInfoAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string> RefreshAsync()
        {
            throw new NotImplementedException();
        }
    }
}