using IdentityAuthentication.Model.Configurations;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityAuthentication.Model
{
    public class TokenValidation
    {
        private readonly TokenConfigurationBase Token;
        private readonly Credentials _credentials;

        public TokenValidation(TokenConfigurationBase token, Credentials credentials)
        {
            Token = token;
            _credentials = credentials;
        }

        public static readonly TokenValidationResult FailedTokenValidationResult = new() { IsValid = false };

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

        public TokenValidationParameters GenerateTokenValidation()
        {
            var signingKey = _credentials.GenerateSignatureSecurityKey();

            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = Token.Issuer,
                ValidateAudience = true,
                ValidAudience = Token.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
            };
        }
    }
}
