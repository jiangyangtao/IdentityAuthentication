using IdentityAuthentication.Model.Configurations;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityAuthentication.Model
{
    public class TokenValidation
    {
        private readonly AccessTokenConfiguration _accessTokenConfig;
        private readonly RefreshTokenConfiguration _refreshTokenConfig;

        private readonly Credentials _credentials;


        public TokenValidation(AccessTokenConfiguration accessTokenConfig, RefreshTokenConfiguration refreshTokenConfig, SecretKeyConfiguration secretKeyConfig, AuthenticationConfiguration authenticationConfig)
        {
            _accessTokenConfig = accessTokenConfig;
            _refreshTokenConfig = refreshTokenConfig;

            _credentials = new Credentials(authenticationConfig, secretKeyConfig);
        }

        /// <summary>
        /// 生成 access_token 的 <see cref="JwtSecurityToken"/>
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="notBefore"></param>
        /// <param name="expirationTime"></param>
        /// <returns></returns>
        public JwtSecurityToken GenerateAccessSecurityToken(Claim[] claims, DateTime notBefore, DateTime expirationTime)
        {
            var signingCredentials = _credentials.GenerateSigningCredentials();

            return new JwtSecurityToken(
                     issuer: _accessTokenConfig.Issuer,
                     audience: _accessTokenConfig.Audience,
                     claims: claims,
                     notBefore: notBefore,
                     expires: expirationTime,
                     signingCredentials: signingCredentials);
        }

        /// <summary>
        /// 生成 refresh_token 的 <see cref="JwtSecurityToken"/>
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public JwtSecurityToken GenerateRefreshSecurityToken(Claim[] claims)
        {
            var signingCredentials = _credentials.GenerateSigningCredentials();

            return new JwtSecurityToken(
                    issuer: _refreshTokenConfig.Issuer,
                    audience: _refreshTokenConfig.Audience,
                    claims: claims,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddDays(_refreshTokenConfig.ExpirationTime),
                    signingCredentials: signingCredentials);
        }

        /// <summary>
        /// 生成 refresh_token 的 <see cref="TokenValidationParameters"/>，用于验签
        /// </summary>
        /// <returns></returns>
        public TokenValidationParameters GenerateRefreshTokenValidation()
        {
            var securityKey = _credentials.GenerateValidateSecurityKey();

            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateIssuer = true,
                ValidIssuer = _refreshTokenConfig.Issuer,
                ValidateAudience = true,
                ValidAudience = _refreshTokenConfig.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
            };
        }

        public TokenValidationParameters GenerateAccessTokenValidation()
        {
            var signingKey = _credentials.GenerateValidateSecurityKey();

            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = _accessTokenConfig.Issuer,
                ValidateAudience = true,
                ValidAudience = _accessTokenConfig.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
            };
        }
    }
}
