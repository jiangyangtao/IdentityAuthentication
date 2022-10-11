using Authentication.Abstractions;
using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using RSAExtensions;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace IdentityAuthentication.Core
{
    internal class TokenProvider
    {
        private readonly AccessTokenConfig accessTokenConfig;
        private readonly RefreshTokenConfig refreshTokenConfig;
        private readonly SecretKeyConfig secretKeyConfig;
        private readonly AuthenticationConfig authenticationConfig;
        private readonly Credentials _credentials;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationHandle _authenticationHandle;

        private readonly string UserIdType = ClaimTypes.Sid;
        private readonly string UsernameType = ClaimTypes.Name;
        private readonly string IssueTimeType = "IssueTime";
        private readonly string ExpirationType = ClaimTypes.Expiration;

        public TokenProvider(
            IOptions<AccessTokenConfig> accessTokenOption,
            IOptions<RefreshTokenConfig> refreshTokenOption,
            IOptions<SecretKeyConfig> secretKeyOption,
            IOptions<AuthenticationConfig> authenticationOption,
            IHttpContextAccessor httpContextAccessor,
            AuthenticationHandle authenticationHandle)
        {
            accessTokenConfig = accessTokenOption.Value;
            refreshTokenConfig = refreshTokenOption.Value;
            secretKeyConfig = secretKeyOption.Value;
            authenticationConfig = authenticationOption.Value;
            _credentials = new Credentials(authenticationConfig, secretKeyConfig);

            _httpContextAccessor = httpContextAccessor;
            _authenticationHandle = authenticationHandle;
        }

        public IToken GenerateToken(AuthenticationResult result)
        {
            var claims = new List<Claim>
            {
                new Claim(UserIdType, result.UserId),
                new Claim(UsernameType,result.Username),
                new Claim(IssueTimeType,DateTime.Now.ToString()),
                new Claim(ExpirationType,TokenExpirationTime.ToString()),
                new Claim(nameof(AuthenticationResult.AuthenticationType),result.AuthenticationType),
                new Claim(nameof(AuthenticationResult.AuthenticationSource),result.AuthenticationSource)
            };

            claims.AddRange(result.GetMetadataClaims());
            var accessToken = BuildAccessToken(claims.ToArray());
            var refreshToken = BuildRefreshToken(result);

            return TokenResult.Create(
                accessToken: accessToken,
                refreshToken: refreshToken,
                expiresIn: accessTokenConfig.ExpirationTime
            );
        }

        public async Task<IToken> RefreshTokenAsync()
        {
            // CheckTokenImmediatelyExpire();

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

            var accessToken = BuildAccessToken(claims.ToArray(), issueTime, expirationTime);
            return TokenResult.Create(accessToken: accessToken);
        }

        private DateTime TokenExpirationTime => DateTime.Now.AddSeconds(accessTokenConfig.ExpirationTime);

        private string BuildAccessToken(Claim[] claims, DateTime? notBefore = null, DateTime? expirationTime = null)
        {
            var signingCredentials = _credentials.GenerateSigningCredentials();

            var securityToken = new JwtSecurityToken(
                    issuer: accessTokenConfig.Issuer,
                    audience: accessTokenConfig.Audience,
                    claims: claims,
                    notBefore: notBefore ?? DateTime.Now,
                    expires: expirationTime ?? TokenExpirationTime,
                    signingCredentials: signingCredentials);
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        private string BuildRefreshToken(AuthenticationResult result)
        {
            var signingCredentials = _credentials.GenerateSigningCredentials();

            var claims = new Claim[]
            {
                new Claim(UserIdType, result.UserId),
                new Claim(UsernameType,result.Username),
                new Claim(nameof(AuthenticationResult.AuthenticationType),result.AuthenticationType),
                new Claim(nameof(AuthenticationResult.AuthenticationSource),result.AuthenticationSource)
            };

            var securityToken = new JwtSecurityToken(
                    issuer: refreshTokenConfig.Issuer,
                    audience: refreshTokenConfig.Audience,
                    claims: claims,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddDays(refreshTokenConfig.ExpirationTime),
                    signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        private void CheckTokenImmediatelyExpire()
        {
            var expirationTime = GetDateTimeClaim(ExpirationType);

            var timeSpan = expirationTime - DateTime.Now;
            if (timeSpan.TotalSeconds > accessTokenConfig.RefreshTime) throw new Exception("Token do not expire immediately");
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
