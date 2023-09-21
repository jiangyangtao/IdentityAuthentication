using Grpc.Core;
using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Application.Grpc.Provider;
using IdentityAuthentication.Core;
using IdentityAuthentication.Model.Enums;
using IdentityAuthentication.Model.Extensions;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace IdentityAuthentication.Application.GrpcServices
{
    [Authorize]
    public class TokenService : TokenGrpcProvider.TokenGrpcProviderBase
    {
        private readonly IIdentityAuthenticationProvider _identityAuthenticationProvider;
        private readonly IAuthenticationConfigurationProvider _authenticationConfigProvider;

        public TokenService(IIdentityAuthenticationProvider identityAuthenticationProvider, IAuthenticationConfigurationProvider authenticationConfigProvider)
        {
            _identityAuthenticationProvider = identityAuthenticationProvider;
            _authenticationConfigProvider = authenticationConfigProvider;
        }

        public async override Task<AuthorizeResponse> Authorize(AccessTokenRequest request, ServerCallContext context)
        {
            var r = await _identityAuthenticationProvider.GetTokenInfoAsync();
            var claims = JsonConvert.SerializeObject(r);
            return new AuthorizeResponse { Result = true, Claims = claims };
        }

        public async override Task<RefreshTokenResponse> Refresh(RefreshTokenRequest request, ServerCallContext context)
        {
            if (_authenticationConfigProvider.Authentication.TokenType == TokenType.JWT)
            {
                var httpContext = context.GetHttpContext();
                httpContext.Request.Headers.SetRefreshToken(request.RefreshToken);
            }

            var token = await _identityAuthenticationProvider.RefreshTokenAsync();
            return new RefreshTokenResponse { AccessToken = token, Result = token.NotNullAndEmpty() };
        }
    }
}
