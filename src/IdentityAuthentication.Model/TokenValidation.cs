using IdentityAuthentication.Model.Configurations;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityAuthentication.Model
{
    public class TokenValidation : IDisposable
    {
        private readonly TokenConfigurationBase Token;
        private readonly Credentials Credentials;

        public TokenValidation(TokenConfigurationBase token, Credentials credentials)
        {
            Token = token;
            Credentials = credentials;
        }

        public static readonly TokenValidationResult FailedTokenValidationResult = new() { IsValid = false };

        /// <summary>
        /// 生成签名的 <see cref="JwtSecurityToken"/>
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="notBefore">签发时间</param>
        /// <param name="expirationTime">过期时间</param>
        /// <returns></returns>
        public JwtSecurityToken GenerateSecurityToken(Claim[] claims, DateTime notBefore, DateTime expirationTime)
        {
            if (claims.IsNullOrEmpty()) throw new ArgumentNullException(nameof(claims));

            var signingCredentials = Credentials.GenerateSigningCredentials();
            return new JwtSecurityToken(
                     issuer: Token.Issuer,
                     audience: Token.Audience,
                     claims: claims,
                     notBefore: notBefore,
                     expires: expirationTime,
                     signingCredentials: signingCredentials);
        }

        /// <summary>
        /// 生成验签的 <see cref="TokenValidationParameters"/>
        /// </summary>
        /// <returns></returns>
        public TokenValidationParameters GenerateTokenValidation()
        {
            var signingKey = Credentials.GenerateSignatureSecurityKey();
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

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
