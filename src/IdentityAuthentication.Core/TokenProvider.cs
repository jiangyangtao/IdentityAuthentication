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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationConfig authenticationConfig;

        public TokenProvider(
            IOptions<AuthenticationConfig> options,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            authenticationConfig = options.Value;
        }

        public IToken GenerateToken(string id, JObject values)
        {
            var claims = BuildClaims(id, values);

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
               expires: TokenExpirationTime,
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

        private DateTime TokenExpirationTime => DateTime.Now.AddSeconds(authenticationConfig.TokenExpirationTime);

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

        public IToken RefreshToken()
        {
            CheckTokenImmediatelyExpire();

            var claims = new List<Claim>();
            foreach (var item in Claims)
            {
                var claim = new Claim(item.Type, item.Value);
                if (item.Type == ClaimTypes.Expiration)
                {
                    claim = new Claim(item.Type, TokenExpirationTime.ToString());
                }

                claims.Add(claim);
            }

            return BuildToken(claims.ToArray());
        }

        /// <summary>
        /// 是否即将过期
        /// </summary>
        private void CheckTokenImmediatelyExpire()
        {
            if (Claims.IsNullOrEmpty()) throw new Exception("Authentication failed");

            var result = Claims.FirstOrDefault(a => a.Type == ClaimTypes.Expiration);
            if (result == null) throw new Exception("Authentication failed");

            var parseResult = DateTime.TryParse(result.Value, out DateTime expirationTime);
            if (parseResult == false) throw new Exception("Authentication failed");

            var timeSpan = expirationTime - DateTime.Now;
            if (timeSpan.TotalSeconds > authenticationConfig.TokenRefreshTime) throw new Exception("Token do not expire immediately");
        }

        private IReadOnlyCollection<Claim> Claims
        {
            get
            {
                if (_httpContextAccessor == null) return Array.Empty<Claim>();
                if (_httpContextAccessor.HttpContext == null) return Array.Empty<Claim>();
                if (_httpContextAccessor.HttpContext.User == null) return Array.Empty<Claim>();

                return _httpContextAccessor.HttpContext.User.Claims.ToArray();
            }
        }

        public Task<IToken> RefreshTokenAsync()
        {
            throw new NotImplementedException();
        }
    }
}
