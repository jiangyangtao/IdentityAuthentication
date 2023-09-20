using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Model;
using IdentityAuthentication.Token.Abstractions;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Parameters;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityAuthentication.Token
{
    internal class TokenSignatureProvider : ITokenSignatureProvider
    {
        private readonly IAuthenticationConfigurationProvider _configurationProvider;
        private readonly Credentials PrivateCredentials;
        private readonly Credentials PublicCredentials;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public TokenSignatureProvider(IAuthenticationConfigurationProvider configurationProvider)
        {
            if (configurationProvider.AccessToken == null) throw new ArgumentException(nameof(configurationProvider.AccessToken));
            if (configurationProvider.RsaSignature == null) throw new ArgumentException(nameof(configurationProvider.RsaSignature));
            if (configurationProvider.Authentication.EnableTokenRefresh && configurationProvider.RefreshToken == null)
                throw new ArgumentException(nameof(configurationProvider.RefreshToken));


            _configurationProvider = configurationProvider;
            PrivateCredentials = configurationProvider.RsaSignature.GetPrivateCredentials();
            PublicCredentials = configurationProvider.RsaSignature.GetPublicCredentials();

            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        #region AccessToken

        private TokenValidation AccessTokenSignature { set; get; }

        public string BuildAccessToken(TokenInfo accessToken)
        {
            AccessTokenSignature ??= new TokenValidation(_configurationProvider.AccessToken, PrivateCredentials);

            var claims = accessToken.BuildClaims();
            var securityToken = AccessTokenSignature.GenerateSecurityToken(claims, accessToken.NotBefore, accessToken.ExpirationTime);
            return _jwtSecurityTokenHandler.WriteToken(securityToken);
        }

        private TokenValidation AccessTokenValidation { set; get; }

        public async Task<TokenValidationResult> ValidateAccessTokenAsync(string token)
        {
            AccessTokenValidation ??= new TokenValidation(_configurationProvider.AccessToken, PublicCredentials);

            var validationParameters = AccessTokenValidation.GenerateTokenValidation();
            return await _jwtSecurityTokenHandler.ValidateTokenAsync(token, validationParameters);
        }

        #endregion

        #region RefreshToken

        private TokenValidation RefreshTokenSignature { set; get; }

        public string BuildRefreshToken(TokenInfo refreshToken)
        {
            if (_configurationProvider.Authentication.EnableTokenRefresh == false) return string.Empty;

            RefreshTokenSignature ??= new TokenValidation(_configurationProvider.RefreshToken, PrivateCredentials);
            var claims = refreshToken.BuildClaims();
            var securityToken = RefreshTokenSignature.GenerateSecurityToken(claims, refreshToken.NotBefore, refreshToken.ExpirationTime);

            return _jwtSecurityTokenHandler.WriteToken(securityToken);
        }

        private TokenValidation RefreshTokenValidation { set; get; }

        public async Task<TokenValidationResult?> ValidateRefreshTokenAsync(string refreshToken)
        {

            if (_configurationProvider.Authentication.EnableTokenRefresh == false) return null;

            RefreshTokenValidation ??= new TokenValidation(_configurationProvider.RefreshToken, PublicCredentials);
            var validationParameters = RefreshTokenValidation.GenerateTokenValidation();
            return await _jwtSecurityTokenHandler.ValidateTokenAsync(refreshToken, validationParameters);
        }

        public TokenValidationParameters? GenerateRefreshTokenValidation()
        {
            if (_configurationProvider.Authentication.EnableTokenRefresh == false) return null;

            RefreshTokenValidation ??= new TokenValidation(_configurationProvider.RefreshToken, PublicCredentials);
            return RefreshTokenValidation.GenerateTokenValidation();
        }

        #endregion
    }
}
