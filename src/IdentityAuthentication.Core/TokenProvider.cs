using IdentityAuthentication.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityAuthentication.Core
{
    internal class TokenProvider : ITokenProvider
    {
        private readonly IConfiguration _configuration;

        private readonly long TokenExpiration;

        public TokenProvider(IConfiguration configuration)
        {
            _configuration = configuration;
            TokenExpiration = configuration.GetValue<long>("Autnentication:TokenExpirationTime");
        }

        public IToken GenerateToken(string id, JObject values)
        {
            var claims = BuildClaims(id, values);

            var signingCredentials = GenerateSigningCredentials();

            var securityToken = new JwtSecurityToken(
               issuer: _configuration.GetValue<string>("Autnentication:Issuer"),
               audience: _configuration.GetValue<string>("Autnentication:Audience"),
               claims: claims,
               notBefore: DateTime.Now,
               expires: TokenExpirationTime,
               signingCredentials: signingCredentials);
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(securityToken);

            var tokenType = _configuration.GetValue<string>("Autnentication:TokenType");
            return TokenResult.CreateReulst(jwtToken, TokenExpiration, tokenType);
        }

        private SigningCredentials GenerateSigningCredentials()
        {
            var secretKey = _configuration.GetValue<string>("Autnentication:Secret");
            var keyByteArray = Encoding.ASCII.GetBytes(secretKey);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            return new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        }

        private TimeSpan TokenExpirationSpan => TimeSpan.FromSeconds(TokenExpiration);

        private DateTime TokenExpirationTime => DateTime.Now.Add(TokenExpirationSpan);

        private Claim[] BuildClaims(string id, JObject values)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, id),
                new Claim(ClaimTypes.Expiration, TokenExpirationTime.ToString())
            };

            foreach (var property in values)
            {
                claims.Add(new Claim(property.Key, property.Value.ToString()));
            }
            return claims.ToArray();
        }

        public Task<IToken> GenerateTokenAsync(string id, JObject values)
        {
            throw new NotImplementedException();
        }

        public IToken RefreshToken(string id, JObject values)
        {
            throw new NotImplementedException();
        }

        public Task<IToken> RefreshTokenAsync(string id, JObject values)
        {
            throw new NotImplementedException();
        }
    }
}
