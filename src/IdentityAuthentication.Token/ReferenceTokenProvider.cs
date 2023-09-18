﻿using IdentityAuthentication.Cache.Abstractions;
using IdentityAuthentication.Configuration.Abstractions;
using IdentityAuthentication.Configuration.Model;
using IdentityAuthentication.Model;
using IdentityAuthentication.Model.Enums;
using IdentityAuthentication.Token.Abstractions;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace IdentityAuthentication.Token
{
    internal class ReferenceTokenProvider : ITokenProvider
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly IHttpTokenProvider _httpTokenProvider;
        private readonly IAuthenticationConfigurationProvider _authenticationConfigurationProvider;

        public ReferenceTokenProvider(
            ICacheProviderFactory cacheProviderFactory,
            IHttpTokenProvider httpTokenProvider,
            IAuthenticationConfigurationProvider authenticationConfigurationProvider)
        {
            _cacheProvider = cacheProviderFactory.CreateCacheProvider();
            _httpTokenProvider = httpTokenProvider;
            _authenticationConfigurationProvider = authenticationConfigurationProvider;
        }

        public TokenType TokenType => TokenType.Reference;

        public async Task<TokenValidationResult> AuthorizeAsync()
        {
            try
            {
                if (_httpTokenProvider.AccessToken.IsNullOrEmpty()) return TokenValidation.FailedTokenValidationResult;

                var data = await _cacheProvider.GetAsync(_httpTokenProvider.AccessToken);
                if (data == null) return TokenValidation.FailedTokenValidationResult;
                if (DateTime.Now > data.ExpirationTime) return TokenValidation.FailedTokenValidationResult;

                var claims = data.GetAuthenticationResult().GetClaims();
                var identity = new ClaimsIdentity(claims, data.GrantType);
                return new TokenValidationResult { IsValid = true, ClaimsIdentity = identity, };
            }
            catch
            {
                return TokenValidation.FailedTokenValidationResult;
            }
        }

        public async Task<string> DestroyAsync()
        {
            if (_httpTokenProvider.AccessToken.IsNullOrEmpty()) return string.Empty;

            await _cacheProvider.RemoveAsync(_httpTokenProvider.AccessToken);
            return _httpTokenProvider.AccessToken;
        }

        public async Task<IToken> GenerateAsync(AuthenticationResult authenticationResult)
        {
            var expirationTime = DateTime.Now.AddSeconds(_authenticationConfigurationProvider.AccessToken.ExpirationTime);
            var token = new ReferenceToken(authenticationResult, expirationTime);

            var accessToken = Guid.NewGuid().ToString();
            await _cacheProvider.SetAsync(accessToken, token);

            return TokenResult.CreateToken(accessToken, _authenticationConfigurationProvider.AccessToken.ExpirationTime, authenticationResult.ToReadOnlyDictionary());
        }

        public async Task<AuthenticationResult> GetAuthenticationResultAsync()
        {
            if (_httpTokenProvider.AccessToken.IsNullOrEmpty()) return null;

            var token = await _cacheProvider.GetAsync(_httpTokenProvider.AccessToken);
            if (token == null) return null;

            return token.GetAuthenticationResult();
        }

        public async Task<IReadOnlyDictionary<string, string>?> GetTokenInfoAsync()
        {
            if (_httpTokenProvider.AccessToken.IsNullOrEmpty()) return null;

            var token = await _cacheProvider.GetAsync(_httpTokenProvider.AccessToken);
            if (token == null) return null;

            return token.ToReadOnlyDictionary();
        }

        public async Task<string> RefreshAsync()
        {
            if (_httpTokenProvider.AccessToken.IsNullOrEmpty()) return string.Empty;
            if (_authenticationConfigurationProvider.Authentication.EnableTokenRefresh == false) return string.Empty;

            var token = await _cacheProvider.GetAsync(_httpTokenProvider.AccessToken);
            token.ExpirationTime = token.ExpirationTime.AddSeconds(_authenticationConfigurationProvider.AccessToken.ExpirationTime);

            await _cacheProvider.SetAsync(_httpTokenProvider.AccessToken, token);
            return _httpTokenProvider.AccessToken;
        }
    }
}
