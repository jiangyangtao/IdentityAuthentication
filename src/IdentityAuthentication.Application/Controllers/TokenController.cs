using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Application.Dto;
using IdentityAuthentication.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace IdentityAuthentication.Application.Controllers
{
    public class TokenController : BaseController
    {
        private readonly IAuthenticationProvider _identityAuthenticationProvider;
        private readonly IAuthenticationConfigurationProvider _authenticationConfigProvider;

        public TokenController(IAuthenticationProvider identityAuthenticationProvider, IAuthenticationConfigurationProvider authenticationConfigProvider)
        {
            _identityAuthenticationProvider = identityAuthenticationProvider;
            _authenticationConfigProvider = authenticationConfigProvider;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(TokenResult), 200)]
        public async Task<IActionResult> Generate([FromBody] JObject credentialData)
        {
            var token = await _identityAuthenticationProvider.AuthenticateAsync(credentialData);
            if (token == null) return new NotFoundResult();

            using var factory = new TokenResultFactory(token, _authenticationConfigProvider.Authentication.TokenType);
            var result = factory.CreateTokenResult();

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(AccessTokenResult), 200)]
        public async Task<IActionResult> Refresh()
        {
            var token = await _identityAuthenticationProvider.RefreshTokenAsync();
            return Ok(new AccessTokenResult(token));
        }

        [HttpPost]
        public async Task<IActionResult> Authorize()
        {
            var result = await _identityAuthenticationProvider.GetTokenInfoAsync();
            return Ok(result);
        }
    }
}
