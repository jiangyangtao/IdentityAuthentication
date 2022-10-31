using Authentication.Abstractions;
using IdentityAuthenticaion.Model;
using IdentityAuthenticaion.Model.Configurations;
using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityAuthentication.Core
{
    internal class TokenProvider
    {
        private readonly AccessTokenConfiguration accessTokenConfig;
        private readonly TokenValidation _tokenValidation;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationHandle _authenticationHandle;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        private readonly string UserIdType = ClaimTypes.Sid;
        private readonly string UsernameType = ClaimTypes.Name;
        private readonly string IssueTimeType = "IssueTime";
        private readonly string ExpirationType = ClaimTypes.Expiration;

        public TokenProvider(
            IOptions<AccessTokenConfiguration> accessTokenOption,
            IOptions<RefreshTokenConfiguration> refreshTokenOption,
            IOptions<SecretKeyConfiguration> secretKeyOption,
            IOptions<AuthenticationConfiguration> authenticationOption,
            IHttpContextAccessor httpContextAccessor,
            AuthenticationHandle authenticationHandle)
        {
            accessTokenConfig = accessTokenOption.Value;
            _tokenValidation = new TokenValidation(accessTokenConfig, refreshTokenOption.Value, secretKeyOption.Value, authenticationOption.Value);
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

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
            await CheckRefreshTokenAsync();

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
            var securityToken = _tokenValidation.GenerateAccessSecurityToken(claims, notBefore ?? DateTime.Now, expirationTime ?? TokenExpirationTime);
            return _jwtSecurityTokenHandler.WriteToken(securityToken);
        }

        private string BuildRefreshToken(AuthenticationResult result)
        {
            var claims = new Claim[]
            {
                new Claim(UserIdType, result.UserId),
                new Claim(UsernameType,result.Username),
                new Claim(nameof(AuthenticationResult.AuthenticationType),result.AuthenticationType),
                new Claim(nameof(AuthenticationResult.AuthenticationSource),result.AuthenticationSource)
            };

            var securityToken = _tokenValidation.GenerateRefreshSecurityToken(claims);
            return _jwtSecurityTokenHandler.WriteToken(securityToken);
        }

        private async Task CheckRefreshTokenAsync()
        {
            // CheckTokenImmediatelyExpire();       

            var token = HttpContent.GetRefreshToken();
            if (token.IsNullOrEmpty()) throw new Exception("refresh-token not detected in header");

            var validationParameters = _tokenValidation.GenerateRefreshTokenValidation();
            var tokenValidationResult = await _jwtSecurityTokenHandler.ValidateTokenAsync(token, validationParameters);
            if (tokenValidationResult.IsValid == false) throw new Exception("Refresh token validation failed");

            foreach (var refreshClaim in tokenValidationResult.Claims)
            {
                var accessClaim = Claims.FirstOrDefault(a => a.Type == refreshClaim.Key);
                if (accessClaim == null) throw new Exception("Refresh token validation failed");
                if (refreshClaim.Value.ToString() != accessClaim.Value) throw new Exception("Refresh token validation failed");
            }
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
                var claimsPrincipal = HttpContent.User;
                if (claimsPrincipal == null) throw new Exception("Authentication failed");

                var claims = claimsPrincipal.Claims.ToArray();
                if (claims.IsNullOrEmpty()) throw new Exception("Authentication failed");

                return claims;
            }
        }

        private HttpContext HttpContent
        {
            get
            {
                if (_httpContextAccessor == null) throw new Exception("Authentication failed");
                if (_httpContextAccessor.HttpContext == null) throw new Exception("Authentication failed");

                return _httpContextAccessor.HttpContext;
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
