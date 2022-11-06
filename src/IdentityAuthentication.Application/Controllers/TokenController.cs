using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Application.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace IdentityAuthentication.Application.Controllers
{
    public class TokenController : BaseController
    {
        private readonly IAuthenticationProvider _authenticationProvider;

        public TokenController(IAuthenticationProvider authenticationProvider)
        {
            _authenticationProvider = authenticationProvider;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(TokenDto), 200)]
        public async Task<IActionResult> Generate()
        {
            var streamReader = new StreamReader(Request.Body);
            var data = await streamReader.ReadToEndAsync();
            var credentialData = JObject.Parse(data);
            var token = await _authenticationProvider.AuthenticateAsync(credentialData);
            if (token == null) return new NotFoundResult();

            return Ok(new TokenDto(token));
        }

        [HttpPost]
        [ProducesResponseType(typeof(TokenDtoBase), 200)]
        public async Task<IActionResult> Refresh()
        {
            var token = await _authenticationProvider.RefreshTokenAsync();
            return Ok(new TokenDtoBase(token));
        }

        [HttpPost]
        public IActionResult Authorize() => Ok();


        [HttpGet]
        public async Task<IActionResult> Info()
        {
            var obj = await _authenticationProvider.TokenInfoAsync();
            if (obj == null) return Ok(new { });

            return Ok(obj);
        }
    }
}
