using IdentityAuthentication.Configuration.Abstractions;
using IdentityAuthentication.Model;
using IdentityAuthentication.Token.Abstractions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityAuthentication.Token
{
    internal class TokenSignatureProvider : ITokenSignatureProvider
    {
        private readonly IAuthenticationConfigurationProvider _configurationProvider;
        private readonly Credentials PrivateCredentials;
        private readonly Credentials PublicCredentials;

        public TokenSignatureProvider(IAuthenticationConfigurationProvider configurationProvider)
        {
            if (configurationProvider.AccessToken == null) throw new ArgumentException(nameof(configurationProvider.AccessToken));
            if (configurationProvider.RsaSignature == null) throw new ArgumentException(nameof(configurationProvider.RsaSignature));
            if (configurationProvider.Authentication.EnableTokenRefresh && configurationProvider.RefreshToken == null)
                throw new ArgumentException(nameof(configurationProvider.RefreshToken));


            _configurationProvider = configurationProvider;
            PrivateCredentials = configurationProvider.RsaSignature.GetPrivateCredentials();
            PublicCredentials = configurationProvider.RsaSignature.GetPublicCredentials();
        }

        #region AccessToken    

        private TokenValidation AccessTokenSignature { set; get; }

        public JwtSecurityToken GenerateAccessSecurityToken(Claim[] claims, DateTime notBefore, DateTime expirationTime)
        {
            AccessTokenSignature ??= new TokenValidation(_configurationProvider.AccessToken, PrivateCredentials);
            return AccessTokenSignature.GenerateSecurityToken(claims, notBefore, expirationTime);
        }


        private TokenValidation AccessTokenValidation { set; get; }

        public TokenValidationParameters GenerateAccessTokenValidation()
        {
            AccessTokenValidation ??= new TokenValidation(_configurationProvider.AccessToken, PublicCredentials);
            return AccessTokenValidation.GenerateTokenValidation();
        }

        #endregion

        #region RefreshToken

        private TokenValidation RefreshTokenSignature { set; get; }

        public JwtSecurityToken? GenerateRefreshSecurityToken(Claim[] claims, DateTime notBefore, DateTime expirationTime)
        {
            if (_configurationProvider.Authentication.EnableTokenRefresh == false) return null;

            RefreshTokenSignature ??= new TokenValidation(_configurationProvider.RefreshToken, PrivateCredentials);
            return RefreshTokenSignature.GenerateSecurityToken(claims, notBefore, expirationTime);
        }

        private TokenValidation RefreshTokenValidation { set; get; }

        public TokenValidationParameters? GenerateRefreshTokenValidation()
        {
            if (_configurationProvider.Authentication.EnableTokenRefresh == false) return null;

            RefreshTokenValidation ??= new TokenValidation(_configurationProvider.RefreshToken, PublicCredentials);
            return RefreshTokenValidation.GenerateTokenValidation();
        }

        #endregion
    }
}
