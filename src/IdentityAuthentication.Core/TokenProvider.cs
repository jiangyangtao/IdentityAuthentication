using Authentication.Abstractions;
using IdentityAuthentication.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RSAExtensions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace IdentityAuthentication.Core
{
    internal class TokenProvider
    {
        private readonly SecretKeyConfig secretKeyConfig;
        private readonly AuthenticationConfig authenticationConfig;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationHandle _authenticationHandle;

        private readonly string UserIdType = ClaimTypes.Sid;
        private readonly string UsernameType = ClaimTypes.Name;
        private readonly string IssueTimeType = "IssueTime";
        private readonly string ExpirationType = ClaimTypes.Expiration;

        public TokenProvider(
            IOptions<SecretKeyConfig> secretKeyOption,
            IOptions<AuthenticationConfig> authenticationOption,
            IHttpContextAccessor httpContextAccessor,
            AuthenticationHandle authenticationHandle)
        {
            secretKeyConfig = secretKeyOption.Value;
            authenticationConfig = authenticationOption.Value;

            _httpContextAccessor = httpContextAccessor;
            _authenticationHandle = authenticationHandle;
        }

        public IToken GenerateToken(AuthenticationResult result)
        {
            var claims = BuildClaim(result);
            return BuildToken(claims);
        }

        public async Task<IToken> RefreshTokenAsync()
        {
            CheckTokenImmediatelyExpire();

            var expirationTime = TokenExpirationTime;
            var issueTime = GetDateTimeClaim(IssueTimeType);
            var authenticationResult = GetAuthenticationResult();
            var checkResult = await _authenticationHandle.IdentityCheckAsync(authenticationResult);
            if (checkResult == false) expirationTime = issueTime.AddSeconds(1);

            var claims = new List<Claim>();
            foreach (var item in Claims)
            {
                claims.Add(item.Clone());
            }

            return BuildToken(claims.ToArray(), issueTime, expirationTime);
        }

        private DateTime TokenExpirationTime => DateTime.Now.AddSeconds(authenticationConfig.TokenExpirationTime);

        private Claim[] BuildClaim(AuthenticationResult result)
        {
            var claims = new List<Claim>
            {
                new Claim(UserIdType, result.UserId),
                new Claim(UsernameType,result.Username),
                new Claim(IssueTimeType,DateTime.Now.ToString()),
                new Claim(ExpirationType,TokenExpirationTime.ToString()),
            };

            foreach (var item in result.Metadata)
            {
                claims.Add(new Claim(item.Key, item.Value));
            }
            return claims.ToArray();
        }

        private TokenResult BuildToken(Claim[] claims, DateTime? notBefore = null, DateTime? expirationTime = null)
        {
            var signingCredentials = GenerateSigningCredentials();

            var securityToken = new JwtSecurityToken(
                    issuer: authenticationConfig.AccessIssuer,
                    audience: authenticationConfig.Audience,
                    claims: claims,
                    notBefore: notBefore ?? DateTime.Now,
                    expires: expirationTime ?? TokenExpirationTime,
                    signingCredentials: signingCredentials);
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return TokenResult.CreateReulst(jwtToken, authenticationConfig.TokenExpirationTime, JwtBearerDefaults.AuthenticationScheme);
        }

        private SigningCredentials GenerateSigningCredentials()
        {
            var key = secretKeyConfig.HmacSha256Key;
            if (authenticationConfig.EncryptionAlgorithm == SecurityAlgorithms.RsaSha256) key = secretKeyConfig.RsaPrivateKey;

            return SecurityKeyHandle.GetSigningCredentials(authenticationConfig.EncryptionAlgorithm, key, true);
        }

        private void CheckTokenImmediatelyExpire()
        {
            var expirationTime = GetDateTimeClaim(ExpirationType);

            var timeSpan = expirationTime - DateTime.Now;
            if (timeSpan.TotalSeconds > authenticationConfig.TokenRefreshTime) throw new Exception("Token do not expire immediately");
        }

        private DateTime GetDateTimeClaim(string type)
        {
            var claim = Claims.FirstOrDefault(a => a.Type == type);
            if (claim == null) throw new Exception("Authentication failed");

            var parseResult = DateTime.TryParse(claim.Value, out DateTime time);
            if (parseResult == false) throw new Exception("Authentication failed");

            return time;
        }

        private IReadOnlyCollection<Claim> Claims
        {
            get
            {
                if (_httpContextAccessor == null) throw new Exception("Authentication failed");
                if (_httpContextAccessor.HttpContext == null) throw new Exception("Authentication failed");
                if (_httpContextAccessor.HttpContext.User == null) throw new Exception("Authentication failed");

                var claims = _httpContextAccessor.HttpContext.User.Claims.ToArray();
                if (claims.IsNullOrEmpty()) throw new Exception("Authentication failed");
                return claims;
            }
        }

        private AuthenticationResult GetAuthenticationResult()
        {
            var userIdClaim = Claims.FirstOrDefault(a => a.Type == UserIdType);
            if (userIdClaim == null) throw new Exception("Authentication failed");

            var usernameClaim = Claims.FirstOrDefault(a => a.Type == UsernameType);
            if (usernameClaim == null) throw new Exception("Authentication failed");

            var authenticationTypeClaim = Claims.FirstOrDefault(a => a.Type == nameof(AuthenticationResult.AuthenticationType));
            if (authenticationTypeClaim == null) throw new Exception("Authentication failed");

            var authenticationSourceClaim = Claims.FirstOrDefault(a => a.Type == nameof(AuthenticationResult.AuthenticationSource));
            if (authenticationSourceClaim == null) throw new Exception("Authentication failed");

            return AuthenticationResult.CreateAuthenticationResult(
                userId: userIdClaim.Value,
                username: usernameClaim.Value,
                authenticationSource: authenticationSourceClaim.Value,
                authenticationType: authenticationTypeClaim.Value,
                metadata: null);
        }
    }
}
