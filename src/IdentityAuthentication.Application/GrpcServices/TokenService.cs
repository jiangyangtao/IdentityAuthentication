using Grpc.Core;
using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Application.Grpc.Provider;
using IdentityAuthentication.Model.Configurations;
using IdentityAuthentication.Model.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace IdentityAuthentication.Application.GrpcServices
{
    [Authorize]
    public class TokenService : TokenGrpcProvider.TokenGrpcProviderBase
    {
        private readonly IAuthenticationProvider _authenticaionProvider;
        private readonly AuthenticationConfigurationBase _authenticationConfig;

        public TokenService(IAuthenticationProvider authenticaionProvider, IOptions<AuthenticationConfigurationBase> authenticationOptions)
        {
            _authenticaionProvider = authenticaionProvider;
            _authenticationConfig = authenticationOptions.Value;
        }

        public async override Task<AuthorizeResponse> Authorize(AccessTokenRequest request, ServerCallContext context)
        {
            var r = await _authenticaionProvider.GetTokenInfoAsync();
            var claims = JsonConvert.SerializeObject(r);
            return new AuthorizeResponse { Result = true, Claims = claims };
        }

        public async override Task<RefreshTokenResponse> Refresh(RefreshTokenRequest request, ServerCallContext context)
        {
            if (_authenticationConfig.TokenType == TokenType.JWT)
            {
                var httpContext = context.GetHttpContext();
                httpContext.Request.Headers.SetRefreshToken(request.RefreshToken);
            }

            var token = await _authenticaionProvider.RefreshTokenAsync();
            return new RefreshTokenResponse { AccessToken = token, Result = token.NotNullAndEmpty() };
        }
    }
}
