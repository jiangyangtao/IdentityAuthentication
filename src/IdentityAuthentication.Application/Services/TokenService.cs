using Grpc.Core;
using IdentityAuthentication.Application.Protos.Token;
using Microsoft.AspNetCore.Authorization;

namespace IdentityAuthentication.Application.Services
{
    [Authorize]
    public class TokenService : TokenProto.TokenProtoBase
    {
        public TokenService()
        {
        }

        public override Task<InfoResponse> Info(AccessTokenModel request, ServerCallContext context)
        {
            return base.Info(request, context);
        }

        public override Task<AuthorizeResponse> Authorize(AccessTokenModel request, ServerCallContext context)
        {    
            return base.Authorize(request, context);
        }

        public override Task<RefreshTokenResponse> Refresh(RefreshTokenModel request, ServerCallContext context)
        {
            var httpContext = context.GetHttpContext();
            httpContext.Request.Headers.TryAdd("refresh-token", request.Token);

            return base.Refresh(request, context);
        }
    }
}
