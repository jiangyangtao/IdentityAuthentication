using Authentication.Abstractions;
using IdentityAuthenticaion.Model;
using IdentityAuthenticaion.Model.Configurations;
using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Extensions;
using IdentityAuthentication.TokenServices.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityAuthentication.TokenServices.Providers
{
    internal class JwtTokenProvider : ITokenProvider
    {
        private readonly AccessTokenConfiguration accessTokenConfig;
        private readonly AuthenticationConfiguration authenticationConfig;
        private readonly TokenValidation _tokenValidation;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        private readonly string IssueTimeKey = "IssueTime";
        private readonly string ExpirationKey = ClaimTypes.Expiration;

        public JwtTokenProvider(
            IOptions<AccessTokenConfiguration> accessTokenOption,
            IOptions<RefreshTokenConfiguration> refreshTokenOption,
            IOptions<SecretKeyConfiguration> secretKeyOption,
            IOptions<AuthenticationConfiguration> authenticationOption,
            IHttpContextAccessor httpContextAccessor)
        {
            accessTokenConfig = accessTokenOption.Value;
            authenticationConfig = authenticationOption.Value;
            _tokenValidation = new TokenValidation(accessTokenConfig, refreshTokenOption.Value, secretKeyOption.Value, authenticationConfig);
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            _httpContextAccessor = httpContextAccessor;
        }

        public TokenType TokenType => TokenType.JWT;

        public Task<IToken> GenerateAsync(AuthenticationResult result)
        {
            var claims = new List<Claim>
            {
                new Claim(IssueTimeKey,DateTime.Now.ToString()),
                new Claim(ExpirationKey,TokenExpirationTime.ToString()),
            };

            claims.AddRange(result.GetClaims());
            var accessToken = BuildAccessToken(claims.ToArray());
            var refreshToken = BuildRefreshToken(result);

            var token = (IToken)TokenResult.Create(
                accessToken: accessToken,
                refreshToken: refreshToken,
                expiresIn: accessTokenConfig.ExpirationTime
            );

            return Task.FromResult(token);
        }

        public Task<string> DestroyAsync()
        {
            var issueTime = GetDateTimeClaim(IssueTimeKey);
            var expirationTime = issueTime.AddSeconds(1);

            var claims = new List<Claim>();
            foreach (var item in Claims)
            {
                claims.Add(item.Clone());
            }

            var accessToken = BuildAccessToken(claims.ToArray(), issueTime, expirationTime);
            return Task.FromResult(accessToken);
        }

        public async Task<string> RefreshAsync()
        {
            await CheckRefreshTokenAsync();

            var expirationTime = TokenExpirationTime;
            var issueTime = GetDateTimeClaim(IssueTimeKey);

            var claims = new List<Claim>();
            foreach (var item in Claims)
            {
                claims.Add(item.Clone());
            }

            var accessToken = BuildAccessToken(claims.ToArray(), issueTime, expirationTime);
            return accessToken;
        }

        public Task<AuthenticationResult> GetAuthenticationResultAsync()
        {
            var userIdClaim = Claims.FirstOrDefault(a => a.Type == AuthenticationResult.UserIdPropertyName);
            if (userIdClaim == null) throw new Exception("Authentication failed");

            var usernameClaim = Claims.FirstOrDefault(a => a.Type == AuthenticationResult.UsernamePropertyName);
            if (usernameClaim == null) throw new Exception("Authentication failed");

            var grantTypeClaim = Claims.FirstOrDefault(a => a.Type == AuthenticationResult.GrantTypePropertyName);
            if (grantTypeClaim == null) throw new Exception("Authentication failed");

            var grantSourceClaim = Claims.FirstOrDefault(a => a.Type == AuthenticationResult.GrantSourcePropertyName);
            if (grantSourceClaim == null) throw new Exception("Authentication failed");

            var clientClaim = Claims.FirstOrDefault(a => a.Type == AuthenticationResult.ClientPropertyName);
            if (clientClaim == null) throw new Exception("Authentication failed");

            var result = AuthenticationResult.CreateAuthenticationResult(
                userId: userIdClaim.Value,
                username: usernameClaim.Value,
                grantSource: grantSourceClaim.Value,
                grantType: grantTypeClaim.Value,
                client: clientClaim.Value,
                metadata: null);

            return Task.FromResult(result);
        }

        private DateTime TokenExpirationTime => DateTime.Now.AddSeconds(accessTokenConfig.ExpirationTime);

        private string BuildAccessToken(Claim[] claims, DateTime? notBefore = null, DateTime? expirationTime = null)
        {
            var securityToken = _tokenValidation.GenerateAccessSecurityToken(claims, notBefore ?? DateTime.Now, expirationTime ?? TokenExpirationTime);
            return _jwtSecurityTokenHandler.WriteToken(securityToken);
        }

        private string BuildRefreshToken(AuthenticationResult result)
        {
            var claims = result.GetClaims();
            var securityToken = _tokenValidation.GenerateRefreshSecurityToken(claims);
            return _jwtSecurityTokenHandler.WriteToken(securityToken);
        }

        private async Task CheckRefreshTokenAsync()
        {
            CheckTokenImmediatelyExpire();

            var token = HttpContent.Request.GetRefreshToken();
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
            var expirationTime = GetDateTimeClaim(ExpirationKey);

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

    }
}
