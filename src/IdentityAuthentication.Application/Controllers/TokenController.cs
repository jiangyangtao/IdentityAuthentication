using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Application.Dto;
using IdentityAuthentication.Core.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace IdentityAuthentication.Application.Controllers
{
    public class TokenController : BaseController
    {
        private readonly IAuthenticationProvider _authenticationProvider;
        private readonly IAuthenticationConfigurationProvider _authenticationConfigProvider;

        public TokenController(IAuthenticationProvider authenticationProvider, IAuthenticationConfigurationProvider authenticationConfigProvider)
        {
            _authenticationProvider = authenticationProvider;
            _authenticationConfigProvider = authenticationConfigProvider;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(TokenResult), 200)]
        public async Task<IActionResult> Generate([FromBody] JObject credentialData)
        {
            var token = await _authenticationProvider.AuthenticateAsync(credentialData);
            if (token == null) return new NotFoundResult();

            using var factory = new TokenResultFactory(token, _authenticationConfigProvider.Authentication.TokenType);
            var result = factory.CreateTokenResult();

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(AccessTokenResult), 200)]
        public async Task<IActionResult> Refresh()
        {
            var token = await _authenticationProvider.RefreshTokenAsync();
            return Ok(new AccessTokenResult(token));
        }

        [HttpPost]
        public async Task<IActionResult> Authorize()
        {
            var result = await _authenticationProvider.GetTokenInfoAsync();
            return Ok(result);
        }
    }
}
