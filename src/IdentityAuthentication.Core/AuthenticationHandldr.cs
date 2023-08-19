﻿using IdentityAuthentication.TokenServices.Abstractions;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace IdentityAuthentication.Core
{
    internal class AuthenticationHandldr : IAuthenticationHandler
    {
        private readonly AuthenticationHandle _authenticationHandle;
        private readonly ITokenProvider _tokenProvider;

        public AuthenticationHandldr(
            AuthenticationHandle authenticationHandle,
           ITokenProviderFactory tokenProviderFactory)
        {
            _tokenProvider = tokenProviderFactory.CreateTokenService();
            _authenticationHandle = authenticationHandle;
        }

        public async Task<ITokenResult> AuthenticateAsync(JObject credentialObject)
        {
            var authenticationResult = await _authenticationHandle.AuthenticateAsync(credentialObject);
            var token = await _tokenProvider.GenerateAsync(authenticationResult);

            return token;
        }

        public async Task<TokenValidationResult> ValidateTokenAsync(string token) => await _tokenProvider.AuthorizeAsync(token);

        public async Task<string> RefreshTokenAsync()
        {
            var authenticationResult = await _tokenProvider.GetAuthenticationResultAsync() ?? throw new Exception("Authentication failed;");
            var checkResult = await _authenticationHandle.IdentityCheckAsync(authenticationResult);
            var token = checkResult ? await _tokenProvider.RefreshAsync() : await _tokenProvider.DestroyAsync();

            return token;
        }

        public async Task<IReadOnlyDictionary<string, string>> GetTokenInfoAsync()
        {
            var dic = await _tokenProvider.GetTokenInfoAsync();
            if (dic == null) return null;

            return dic;
        }
    }
}