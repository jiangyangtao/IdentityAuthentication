using IdentityAuthentication.Model.Configurations;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityAuthentication.Model
{
    public class TokenValidation
    {
        private readonly TokenBase Token;
        private readonly Credentials _credentials;

        public TokenValidation(TokenBase token, Credentials credentials)
        {
            Token = token;
            _credentials = credentials;
        }

        /// <summary>
        /// 生成 access_token 的 <see cref="JwtSecurityToken"/>
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="notBefore"></param>
        /// <param name="expirationTime"></param>
        /// <returns></returns>
        public JwtSecurityToken GenerateSecurityToken(Claim[] claims, DateTime notBefore, DateTime expirationTime)
        {
            var signingCredentials = _credentials.GenerateSigningCredentials();

            return new JwtSecurityToken(
                     issuer: Token.Issuer,
                     audience: Token.Audience,
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
                    issuer: Token.Issuer,
                    audience: Token.Audience,
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
