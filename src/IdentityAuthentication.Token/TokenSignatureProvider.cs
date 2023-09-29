using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Configuration.Enums;
using IdentityAuthentication.Model;
using IdentityAuthentication.Token.Abstractions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace IdentityAuthentication.Token
{
    internal class TokenSignatureProvider : ITokenSignatureProvider
    {
        private readonly IAuthenticationConfigurationProvider _configurationProvider;
        private readonly Credentials SignatureCredentials;
        private readonly Credentials ValidateCredentials;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public TokenSignatureProvider(IAuthenticationConfigurationProvider configurationProvider)
        {
            if (configurationProvider.AccessToken == null) throw new ArgumentException(nameof(configurationProvider.AccessToken));
            if (configurationProvider.Authentication.EnableTokenRefresh && configurationProvider.RefreshToken == null)
                throw new ArgumentException(nameof(configurationProvider.RefreshToken));

            if (configurationProvider.Authentication.TokenSignatureType == TokenSignatureType.Rsa)
            {
                if (configurationProvider.RsaSignature == null) throw new ArgumentException(nameof(configurationProvider.RsaSignature));
                SignatureCredentials = configurationProvider.RsaSignature.GetPrivateCredentials();
                ValidateCredentials = configurationProvider.RsaSignature.GetPublicCredentials();
            }

            if (configurationProvider.Authentication.TokenSignatureType == TokenSignatureType.Symmetric)
            {
                if (configurationProvider.SymmetricSignature == null) throw new ArgumentException(nameof(configurationProvider.SymmetricSignature));

                SignatureCredentials = new(configurationProvider.SymmetricSignature);
                ValidateCredentials = new(configurationProvider.SymmetricSignature);
            }

            _configurationProvider = configurationProvider;
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        public TokenSignatureType TokenSignatureType => TokenSignatureType.Rsa;

        #region AccessToken

        private TokenValidation AccessTokenSignature { set; get; }

        public string BuildAccessToken(TokenInfo accessToken)
        {
            AccessTokenSignature ??= new TokenValidation(_configurationProvider.AccessToken, SignatureCredentials);

            var claims = accessToken.BuildClaims();
            var securityToken = AccessTokenSignature.GenerateSecurityToken(claims, accessToken.NotBefore, accessToken.ExpirationTime);
            return _jwtSecurityTokenHandler.WriteToken(securityToken);
        }

        private TokenValidation AccessTokenValidation { set; get; }

        public async Task<TokenValidationResult> ValidateAccessTokenAsync(string token)
        {
            AccessTokenValidation ??= new TokenValidation(_configurationProvider.AccessToken, ValidateCredentials);

            var validationParameters = AccessTokenValidation.GenerateTokenValidation();
            return await _jwtSecurityTokenHandler.ValidateTokenAsync(token, validationParameters);
        }

        #endregion

        #region RefreshToken

        private TokenValidation RefreshTokenSignature { set; get; }

        public string BuildRefreshToken(TokenInfo refreshToken)
        {
            if (_configurationProvider.Authentication.EnableTokenRefresh == false) return string.Empty;

            RefreshTokenSignature ??= new TokenValidation(_configurationProvider.RefreshToken, SignatureCredentials);
            var claims = refreshToken.BuildClaims();
            var securityToken = RefreshTokenSignature.GenerateSecurityToken(claims, refreshToken.NotBefore, refreshToken.ExpirationTime);

            return _jwtSecurityTokenHandler.WriteToken(securityToken);
        }

        private TokenValidation RefreshTokenValidation { set; get; }

        public async Task<TokenValidationResult?> ValidateRefreshTokenAsync(string refreshToken)
        {

            if (_configurationProvider.Authentication.EnableTokenRefresh == false) return null;

            RefreshTokenValidation ??= new TokenValidation(_configurationProvider.RefreshToken, ValidateCredentials);
            var validationParameters = RefreshTokenValidation.GenerateTokenValidation();
            return await _jwtSecurityTokenHandler.ValidateTokenAsync(refreshToken, validationParameters);
        }

        public TokenValidationParameters? GenerateRefreshTokenValidation()
        {
            if (_configurationProvider.Authentication.EnableTokenRefresh == false) return null;

            RefreshTokenValidation ??= new TokenValidation(_configurationProvider.RefreshToken, ValidateCredentials);
            return RefreshTokenValidation.GenerateTokenValidation();
        }

        #endregion
    }
}
