using Grpc.Core;
using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Application.Protos.Token;
using IdentityAuthentication.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace IdentityAuthentication.Application.Services
{
    [Authorize]
    public class TokenService : TokenProto.TokenProtoBase
    {
        private readonly IAuthenticationProvider _authenticaionProvider;

        public TokenService(IAuthenticationProvider authenticaionProvider)
        {
            _authenticaionProvider = authenticaionProvider;
        }

        public async override Task<InfoResponse> Info(AccessTokenModel request, ServerCallContext context)
        {
            var obj = await _authenticaionProvider.TokenInfoAsync();

            var r = string.Empty;
            if (obj != null) r = obj.ToString();

            return new InfoResponse { Data = r };
        }

        public async override Task<AuthorizeResponse> Authorize(AccessTokenModel request, ServerCallContext context)
        {
            var r = await _authenticaionProvider.AuthorizeAsync(request.Token);
            return new AuthorizeResponse { Result = r.IsValid };
        }

        public async override Task<RefreshTokenResponse> Refresh(RefreshTokenModel request, ServerCallContext context)
        {
            var httpContext = context.GetHttpContext();
            httpContext.Request.Headers.TryAdd("refresh-token", request.Token);

            var token = await _authenticaionProvider.RefreshTokenAsync();
            return new RefreshTokenResponse { AccessToken = token, Result = token.IsNullOrEmpty() };
        }
    }
}
