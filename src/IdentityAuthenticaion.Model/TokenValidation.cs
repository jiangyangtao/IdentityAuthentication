using IdentityAuthenticaion.Model.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityAuthenticaion.Model
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


        public TokenValidationParameters GenerateRefreshTokenValidation()
        {
            var securityKey = _credentials.GenerateEncryptSecurityKey();

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

        public static TokenValidationParameters BuildAccessTokenValidationParameters(IConfiguration configuration)
        {
            var issuer = configuration.GetValue<string>("AccessToken:Issuer");
            var audience = configuration.GetValue<string>("AccessToken:Audience");
            var signingKey = Credentials.GetEncryptSecurityKey(configuration);

            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
            };
        }
    }
}
