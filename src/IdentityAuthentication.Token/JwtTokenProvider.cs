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
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly ITokenSignatureProvider _tokenSignatureProvider;


        public JwtTokenProvider(
            IAuthenticationConfigurationProvider configurationProvider,
            ITokenSignatureProvider tokenSignatureProvider)
        {
            _configurationProvider = configurationProvider;
            _tokenSignatureProvider = tokenSignatureProvider;
        }

        public TokenType TokenType => TokenType.JWT;

        public async Task<TokenValidationResult> AuthorizeAsync(string token)
        {
            var validationParameters = _tokenSignatureProvider.GenerateAccessTokenValidation();
            var tokenValidationResult = await _jwtSecurityTokenHandler.ValidateTokenAsync(token, validationParameters);
            return tokenValidationResult;
        }

        public Task<string> DestroyAsync(string token)
        {
            var issueTime = GetDateTimeClaim(IdentityAuthenticationDefaultKeys.IssueTime);
            var expirationTime = issueTime.AddSeconds(1);

            var claims = new List<Claim>();
            foreach (var item in Claims)
            {
                claims.Add(item.Clone());
            }

            var accessToken = BuildAccessToken(claims.ToArray(), issueTime, expirationTime);
            return Task.FromResult(accessToken);
        }

        private DateTime GetDateTimeClaim(string type)
        {
            var claim = Claims.FirstOrDefault(a => a.Type == type) ?? throw new Exception("Authentication failed");

            var parseResult = DateTime.TryParse(claim.Value, out DateTime time);
            if (parseResult == false) throw new Exception("Authentication failed");

            return time;
        }

        public Task<ITokenResult> GenerateAsync(AuthenticationResult authenticationResult)
        {
            throw new NotImplementedException();
        }

        public Task<AuthenticationResult> GetAuthenticationResultAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyDictionary<string, string>?> GetTokenInfoAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string> RefreshAsync()
        {
            throw new NotImplementedException();
        }
    }
}