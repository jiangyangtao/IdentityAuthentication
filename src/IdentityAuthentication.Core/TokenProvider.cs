using IdentityAuthentication.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityAuthentication.Core
{
    internal class TokenProvider : ITokenProvider
    {
        private readonly IClaimProvider _claimProvider;
        private readonly AuthenticationConfig authenticationConfig;

        public TokenProvider(IOptions<AuthenticationConfig> options, IClaimProvider claimProvider)
        {
            authenticationConfig = options.Value;
            _claimProvider = claimProvider;
        }

        public IToken GenerateToken(Claim[] claims)
        {
            return BuildToken(claims);
        }

        private TokenResult BuildToken(Claim[] claims)
        {
            var signingCredentials = GenerateSigningCredentials();

            var securityToken = new JwtSecurityToken(
               issuer: authenticationConfig.Issuer,
               audience: authenticationConfig.Audience,
               claims: claims,
               notBefore: DateTime.Now,
               expires: DateTime.Now.AddSeconds(authenticationConfig.TokenExpirationTime),
               signingCredentials: signingCredentials);
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return TokenResult.CreateReulst(jwtToken, authenticationConfig.TokenExpirationTime, JwtBearerDefaults.AuthenticationScheme);
        }

        private SigningCredentials GenerateSigningCredentials()
        {
            var secretKey = authenticationConfig.Secret;
            var keyByteArray = Encoding.ASCII.GetBytes(secretKey);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            return new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        }

        public Task<IToken> GenerateTokenAsync(string id, JObject values)
        {
            throw new NotImplementedException();
        }

        public IToken RefreshToken()
        {
            var claims = _claimProvider.ResetTokenExpiration();
            return BuildToken(claims.ToArray());
        }

        public Task<IToken> RefreshTokenAsync()
        {
            throw new NotImplementedException();
        }
    }
}
