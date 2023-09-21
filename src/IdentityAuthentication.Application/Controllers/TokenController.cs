using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Application.Dto;
using IdentityAuthentication.Core;
using IdentityAuthentication.Model.Configurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace IdentityAuthentication.Application.Controllers
{
    public class TokenController : BaseController
    {
        private readonly IIdentityAuthenticationProvider _identityAuthenticationProvider;
        private readonly AuthenticationConfigurationBase _authenticationConfiguration;

        public TokenController(IOptions<AuthenticationConfigurationBase> authenticationOption, IIdentityAuthenticationProvider identityAuthenticationProvider)
        {
            _authenticationConfiguration = authenticationOption.Value;
            _identityAuthenticationProvider = identityAuthenticationProvider;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(TokenResult), 200)]
        public async Task<IActionResult> Generate([FromBody] JObject credentialData)
        {
            var token = await _authenticationProvider.AuthenticateAsync(credentialData);
            if (token == null) return new NotFoundResult();

            using var factory = new TokenResultFactory(token, _authenticationConfiguration.TokenType);
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
