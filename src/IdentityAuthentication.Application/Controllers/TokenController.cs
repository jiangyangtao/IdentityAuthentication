using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Application.Dto;
using IdentityAuthentication.Model.Configurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace IdentityAuthentication.Application.Controllers
{
    public class TokenController : BaseController
    {
        private readonly IAuthenticationProvider _authenticationProvider;
        private readonly AuthenticationConfiguration _authenticationConfiguration;

        public TokenController(IAuthenticationProvider authenticationProvider, IOptions<AuthenticationConfiguration> authenticationOption)
        {
            _authenticationProvider = authenticationProvider;
            _authenticationConfiguration = authenticationOption.Value;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(TokenResult), 200)]
        public async Task<IActionResult> Generate([FromBody] JObject credentialData)
        {
            var token = await _authenticationProvider.AuthenticateAsync(credentialData);
            if (token == null) return new NotFoundResult();

            var factory = new TokenResultFactory(token, _authenticationConfiguration.TokenType);
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
            var result = await _authenticationProvider.TokenInfoAsync();
            return Ok(result);
        }
    }
}
