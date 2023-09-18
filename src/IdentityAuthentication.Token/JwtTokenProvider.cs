using IdentityAuthentication.Configuration.Abstractions;
using IdentityAuthentication.Configuration.Model;
using IdentityAuthentication.Model.Enums;
using IdentityAuthentication.Model.Handles;
using IdentityAuthentication.Token.Abstractions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityAuthentication.Token
{
    internal class JwtTokenProvider : ITokenProvider
    {
        private readonly IAuthenticationConfigurationProvider _configurationProvider;
        private readonly ITokenSignatureProvider _tokenSignatureProvider;
        private readonly IHttpTokenProvider _httpTokenProvider;


        public JwtTokenProvider(
            IAuthenticationConfigurationProvider configurationProvider,
            ITokenSignatureProvider tokenSignatureProvider,
            IHttpTokenProvider httpTokenProvider)
        {
            _configurationProvider = configurationProvider;
            _tokenSignatureProvider = tokenSignatureProvider;
            _httpTokenProvider = httpTokenProvider;
        }

        public TokenType TokenType => TokenType.JWT;

        private DateTime TokenExpirationTime => DateTime.Now.AddSeconds(_configurationProvider.AccessToken.ExpirationTime);

        public Task<TokenValidationResult> AuthorizeAsync() => _tokenSignatureProvider.ValidateAccessTokenAsync(_httpTokenProvider.AccessToken);

        public Task<string> DestroyAsync()
        {
            var issueTime = GetDateTimeClaim(IdentityAuthenticationDefaultKeys.IssueTime);
            var expirationTime = issueTime.AddSeconds(1);

            var claims = new List<Claim>();
            foreach (var item in _httpTokenProvider.UserClaims)
            {
                claims.Add(item.Clone());
            }

            var accessToken = _tokenSignatureProvider.BuildAccessToken(claims.ToArray(), issueTime, expirationTime);
            return Task.FromResult(accessToken);
        }

        private DateTime GetDateTimeClaim(string type)
        {
            var claim = _httpTokenProvider.UserClaims.FirstOrDefault(a => a.Type == type) ?? throw new Exception("Authentication failed");

            var parseResult = DateTime.TryParse(claim.Value, out DateTime time);
            if (parseResult == false) throw new Exception("Authentication failed");

            return time;
        }

        public Task<IToken> GenerateAsync(AuthenticationResult authenticationResult)
        {
            var claims = new List<Claim>
            {
                new Claim(IdentityAuthenticationDefaultKeys.IssueTime,DateTime.Now.ToString()),
                new Claim(IdentityAuthenticationDefaultKeys.Expiration,TokenExpirationTime.ToString()),
            };

            claims.AddRange(authenticationResult.GetClaims());
            var _claims = claims.ToArray();
            var accessToken = _tokenSignatureProvider.BuildAccessToken(_claims);
            var token = TokenResult.CreateToken(accessToken: accessToken, _configurationProvider.AccessToken.ExpirationTime, authenticationResult.ToReadOnlyDictionary());

            if (_configurationProvider.Authentication.EnableTokenRefresh)
            {
                var refreshToken = _tokenSignatureProvider.BuildRefreshToken(_claims);
                token = TokenResult.CreateToken(accessToken, _configurationProvider.AccessToken.ExpirationTime, authenticationResult.ToReadOnlyDictionary(), refreshToken: refreshToken);
            }

            return Task.FromResult(token);
        }



        public Task<AuthenticationResult> GetAuthenticationResultAsync()
        {
            var userIdClaim = _httpTokenProvider.UserClaims.FirstOrDefault(a => a.Type == AuthenticationResult.UserIdPropertyName) ?? throw new Exception("Authentication failed");
            var usernameClaim = _httpTokenProvider.UserClaims.FirstOrDefault(a => a.Type == AuthenticationResult.UsernamePropertyName) ?? throw new Exception("Authentication failed");
            var grantTypeClaim = _httpTokenProvider.UserClaims.FirstOrDefault(a => a.Type == AuthenticationResult.GrantTypePropertyName) ?? throw new Exception("Authentication failed");
            var grantSourceClaim = _httpTokenProvider.UserClaims.FirstOrDefault(a => a.Type == AuthenticationResult.GrantSourcePropertyName) ?? throw new Exception("Authentication failed");
            var clientClaim = _httpTokenProvider.UserClaims.FirstOrDefault(a => a.Type == AuthenticationResult.ClientPropertyName) ?? throw new Exception("Authentication failed");

            var result = AuthenticationResult.CreateAuthenticationResult(
                userId: userIdClaim.Value,
                username: usernameClaim.Value,
                grantSource: grantSourceClaim.Value,
                grantType: grantTypeClaim.Value,
                client: clientClaim.Value,
                metadata: null);

            return Task.FromResult(result);
        }

        public Task<IReadOnlyDictionary<string, string>?> GetTokenInfoAsync()
        {
            var claiims = (IReadOnlyDictionary<string, string>)_httpTokenProvider.UserClaims.ToDictionary(a => a.Type, a => a.Value);
            return Task.FromResult(claiims);
        }

        public async Task<string> RefreshAsync()
        {
            var r = await CheckRefreshTokenAsync();
            if (r == false) return string.Empty;

            var expirationTime = TokenExpirationTime;
            var issueTime = GetDateTimeClaim(IdentityAuthenticationDefaultKeys.IssueTime);

            var claims = new List<Claim>();
            foreach (var item in _httpTokenProvider.UserClaims)
            {
                if (item.Type.Equals(JwtRegisteredClaimNames.Aud, StringComparison.OrdinalIgnoreCase)) continue;

                claims.Add(item.Clone());
            }

            var accessToken = _tokenSignatureProvider.BuildAccessToken(claims.ToArray(), issueTime, expirationTime);
            return accessToken;
        }

        private async Task<bool> CheckRefreshTokenAsync()
        {
            var expirationTime = GetDateTimeClaim(IdentityAuthenticationDefaultKeys.Expiration);
            var timeSpan = expirationTime - DateTime.Now;
            if (timeSpan.TotalSeconds > _configurationProvider.AccessToken.RefreshTime) return false;

            var refreshToken = _httpTokenProvider.RefreshToken;
            if (refreshToken.IsNullOrEmpty()) return false;

            var tokenValidationResult = await _tokenSignatureProvider.ValidateRefreshTokenAsync(refreshToken);
            if (tokenValidationResult.IsValid == false) return false;

            foreach (var refreshClaim in tokenValidationResult.Claims)
            {
                if (refreshClaim.Key.Equals(JwtRegisteredClaimNames.Exp, StringComparison.OrdinalIgnoreCase)) continue;
                if (refreshClaim.Key.Equals(JwtRegisteredClaimNames.Iss, StringComparison.OrdinalIgnoreCase)) continue;
                if (refreshClaim.Key.Equals(JwtRegisteredClaimNames.Aud, StringComparison.OrdinalIgnoreCase)) continue;

                var accessClaim = _httpTokenProvider.UserClaims.FirstOrDefault(a => a.Type == refreshClaim.Key);
                if (accessClaim == null) return false;
                if (refreshClaim.Value.ToString() != accessClaim.Value) return false;
            }

            return true;
        }
    }
}