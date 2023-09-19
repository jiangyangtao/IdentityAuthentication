﻿using IdentityAuthentication.Configuration.Abstractions;
using IdentityAuthentication.Configuration.Model;
using IdentityAuthentication.Model;
using IdentityAuthentication.Model.Enums;
using IdentityAuthentication.Token.Abstractions;
using Microsoft.IdentityModel.Tokens;

namespace IdentityAuthentication.Token
{
    internal class JwtTokenProvider : ITokenProvider
    {
        private readonly IAuthenticationConfigurationProvider _configurationProvider;
        private readonly ITokenSignatureProvider _tokenSignatureProvider;
        private readonly IHttpTokenProvider _httpTokenProvider;


        public JwtTokenProvider(
            IAuthenticationConfigurationProvider configurationProvider,
            ITokenSignatureProvider tokenSignatureProvider,
            IHttpTokenProvider httpTokenProvider)
        {
            _configurationProvider = configurationProvider;
            _tokenSignatureProvider = tokenSignatureProvider;
            _httpTokenProvider = httpTokenProvider;
        }

        public TokenType TokenType => TokenType.JWT;

        public async Task<TokenValidationResult> AuthorizeAsync()
        {
            if (_httpTokenProvider.AccessTokenIsEmpty) return TokenValidation.FailedTokenValidationResult;

            return await _tokenSignatureProvider.ValidateAccessTokenAsync(_httpTokenProvider.AccessToken);
        }

        public Task<string> DestroyAsync()
        {
            var tokenInfo = _httpTokenProvider.AccessTokenInfo ?? throw new Exception("Authentication failed");

            tokenInfo.ExpirationTime = tokenInfo.IssueTime.AddSeconds(1);
            var accessToken = _tokenSignatureProvider.BuildAccessToken(tokenInfo);
            return Task.FromResult(accessToken);
        }

        public Task<IToken> GenerateAsync(AuthenticationResult authenticationResult)
        {
            var accessTokenInfo = TokenInfo.CreateToken(authenticationResult);

            accessTokenInfo.ExpirationTime = _configurationProvider.AccessToken.TokenExpirationTime;

            var accessToken = _tokenSignatureProvider.BuildAccessToken(accessTokenInfo);
            var token = TokenResult.CreateToken(accessToken: accessToken, _configurationProvider.AccessToken.ExpirationTime, authenticationResult.ToReadOnlyDictionary());

            if (_configurationProvider.Authentication.EnableTokenRefresh)
            {
                var refreshTokenInfo = TokenInfo.CreateToken(authenticationResult);
                refreshTokenInfo.ExpirationTime = _configurationProvider.RefreshToken.TokenExpirationTime;

                var refreshToken = _tokenSignatureProvider.BuildRefreshToken(refreshTokenInfo);
                token = TokenResult.CreateToken(accessToken, _configurationProvider.AccessToken.ExpirationTime, authenticationResult.ToReadOnlyDictionary(), refreshToken: refreshToken);
            }

            return Task.FromResult(token);
        }

        public Task<AuthenticationResult> GetAuthenticationResultAsync()
        {
            var userIdClaim = _httpTokenProvider.UserClaims.FirstOrDefault(a => a.Type == AuthenticationResult.UserIdPropertyName) ?? throw new Exception("Authentication failed");
            var usernameClaim = _httpTokenProvider.UserClaims.FirstOrDefault(a => a.Type == AuthenticationResult.UsernamePropertyName) ?? throw new Exception("Authentication failed");
            var grantTypeClaim = _httpTokenProvider.UserClaims.FirstOrDefault(a => a.Type == AuthenticationResult.GrantTypePropertyName) ?? throw new Exception("Authentication failed");
            var grantSourceClaim = _httpTokenProvider.UserClaims.FirstOrDefault(a => a.Type == AuthenticationResult.GrantSourcePropertyName) ?? throw new Exception("Authentication failed");
            var clientClaim = _httpTokenProvider.UserClaims.FirstOrDefault(a => a.Type == AuthenticationResult.ClientPropertyName) ?? throw new Exception("Authentication failed");

            var result = AuthenticationResult.CreateAuthenticationResult(
                userId: userIdClaim.Value,
                username: usernameClaim.Value,
                grantSource: grantSourceClaim.Value,
                grantType: grantTypeClaim.Value,
                client: clientClaim.Value,
                metadata: null);

            return Task.FromResult(result);
        }

        public Task<IReadOnlyDictionary<string, string>?> GetTokenInfoAsync()
        {
            var claiims = (IReadOnlyDictionary<string, string>)_httpTokenProvider.UserClaims.ToDictionary(a => a.Type, a => a.Value);
            return Task.FromResult(claiims);
        }

        public async Task<string> RefreshAsync()
        {
            if (_configurationProvider.Authentication.EnableTokenRefresh == false) return string.Empty;

            var r = await ValidateRefreshTokenAsync();
            if (r == false) return string.Empty;

            var accessTokenInfo = _httpTokenProvider.AccessTokenInfo;
            accessTokenInfo.ExpirationTime = _configurationProvider.AccessToken.TokenExpirationTime;

            var accessToken = _tokenSignatureProvider.BuildAccessToken(accessTokenInfo);
            return accessToken;
        }

        private async Task<bool> ValidateRefreshTokenAsync()
        {
            var accessTokenInfo = _httpTokenProvider.AccessTokenInfo;
            if (accessTokenInfo == null) return false;

            var timeSpan = accessTokenInfo.ExpirationTime - DateTime.Now;
            if (timeSpan.TotalSeconds > _configurationProvider.AccessToken.RefreshTime) return false;

            var refreshToken = _httpTokenProvider.RefreshToken;
            if (refreshToken.IsNullOrEmpty()) return false;

            var tokenValidationResult = await _tokenSignatureProvider.ValidateRefreshTokenAsync(refreshToken);
            if (tokenValidationResult.IsValid == false) return false;

            var r = TokenInfo.TryParse(tokenValidationResult.Claims, out TokenInfo? refreshTokenInfo);
            if (r == false) return false;
            if (refreshTokenInfo == null) return false;

            return refreshTokenInfo.Equals(accessTokenInfo);
        }
    }
}