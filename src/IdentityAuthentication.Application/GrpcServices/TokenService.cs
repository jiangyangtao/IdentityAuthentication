using Grpc.Core;
using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Application.Protos.Token;
using IdentityAuthentication.Extensions;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace IdentityAuthentication.Application.GrpcServices
{
    [Authorize]
    public class TokenService : TokenProto.TokenProtoBase
    {
        private readonly IAuthenticationProvider _authenticaionProvider;

        public TokenService(IAuthenticationProvider authenticaionProvider)
        {
            _authenticaionProvider = authenticaionProvider;
        }

        public async override Task<AuthorizeResponse> Authorize(TokenRequest request, ServerCallContext context)
        {
            var r = await _authenticaionProvider.ValidateTokenAsync(request.Token);
            var dic = r.ClaimsIdentity.Claims.ToDictionary(a => a.Type, a => a.Value);
            var claims = JsonConvert.SerializeObject(dic);
            return new AuthorizeResponse { Result = r.IsValid, Claims = claims };
        }

        public async override Task<RefreshTokenResponse> Refresh(TokenRequest request, ServerCallContext context)
        {
            var httpContext = context.GetHttpContext();
            httpContext.Request.Headers.TryAdd("refresh-token", request.Token);

            var token = await _authenticaionProvider.RefreshTokenAsync();
            return new RefreshTokenResponse { AccessToken = token, Result = token.IsNullOrEmpty() };
        }
    }
}
