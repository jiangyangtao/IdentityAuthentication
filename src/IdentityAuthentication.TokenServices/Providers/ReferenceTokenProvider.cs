﻿using Authentication.Abstractions;
using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Model;
using IdentityAuthentication.Model.Configurations;
using IdentityAuthentication.TokenServices.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace IdentityAuthentication.TokenServices.Providers
{
    internal class ReferenceTokenProvider : ITokenProvider
    {
        private readonly TokenValidation _tokenValidation;
        private readonly AccessTokenConfiguration accessTokenConfig;

        private readonly ICacheProvider _cacheProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReferenceTokenProvider(
            ICacheProviderFactory cacheProviderFactory,
            IHttpContextAccessor httpContextAccessor,
            IOptions<AccessTokenConfiguration> accessTokenOption,
            IOptions<RefreshTokenConfiguration> refreshTokenOption,
            IOptions<SecretKeyConfiguration> secretKeyOption,
            IOptions<AuthenticationConfiguration> authenticationOption)
        {

            _cacheProvider = cacheProviderFactory.CreateCacheProvider();
            _httpContextAccessor = httpContextAccessor;
            accessTokenConfig = accessTokenOption.Value;

            _tokenValidation = new TokenValidation(accessTokenConfig, refreshTokenOption.Value, secretKeyOption.Value, authenticationOption.Value);
        }

        public TokenType TokenType => TokenType.Reference;

        public async Task<TokenValidationResult> AuthorizeAsync(string token)
        {
            var data = await _cacheProvider.GetAsync(token);
            if (data == null) return new TokenValidationResult { IsValid = false };
            if (DateTime.Now < data.ExpirationTime) return new TokenValidationResult { IsValid = false };

            var claims = data.GetAuthenticationResult().GetClaims();
            var identity = new ClaimsIdentity(claims, data.GrantType);
            return new TokenValidationResult
            {
                IsValid = true,
                ClaimsIdentity = identity,
                SecurityToken = _tokenValidation.GenerateAccessSecurityToken(claims, data.ExpirationTime.AddHours(-1), data.ExpirationTime),
            };
        }

        public async Task<string> DestroyAsync()
        {
            await _cacheProvider.RemoveAsync(Authentication);
            return Authentication;
        }

        public async Task<IToken> GenerateAsync(AuthenticationResult authenticationResult)
        {
            var expirationTime = DateTime.Now.AddSeconds(accessTokenConfig.ExpirationTime);
            var token = new ReferenceToken(authenticationResult, expirationTime);

            var accessToken = Guid.NewGuid().ToString();
            await _cacheProvider.SetAsync(accessToken, token);

            return TokenResult.Create(accessToken);
        }

        public async Task<AuthenticationResult> GetAuthenticationResultAsync()
        {
            var token = await _cacheProvider.GetAsync(Authentication);
            if (token == null) return null;

            return token.GetAuthenticationResult();
        }

        public async Task<IReadOnlyDictionary<string, string>?> InfoAsync()
        {
            var token = await _cacheProvider.GetAsync(Authentication);
            if (token == null) return null;

            return token.ToReadOnlyDictionary();
        }

        public async Task<string> RefreshAsync()
        {
            var token = await _cacheProvider.GetAsync(Authentication);
            token.ExpirationTime = token.ExpirationTime.AddSeconds(accessTokenConfig.ExpirationTime);

            await _cacheProvider.SetAsync(Authentication, token);
            return Authentication;
        }

        private HttpRequest HttpRequest
        {
            get
            {
                if (_httpContextAccessor == null) throw new Exception("Authentication failed");
                if (_httpContextAccessor.HttpContext == null) throw new Exception("Authentication failed");

                return _httpContextAccessor.HttpContext.Request;
            }
        }

        private string Authentication
        {
            get
            {
                var authorization = HttpRequest.Headers.Authorization;
                if (StringValues.IsNullOrEmpty(authorization)) throw new Exception("Authentication failed");

                return authorization;
            }
        }
    }
}