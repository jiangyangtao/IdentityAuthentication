using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Application.Dto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace IdentityAuthentication.Application.Controllers
{
    [Route("v1/api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationProvider _authenticationProvider;

        public AuthenticationController(IAuthenticationProvider authenticationProvider)
        {
            _authenticationProvider = authenticationProvider;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate()
        {
            var streamReader = new StreamReader(Request.Body);
            var data = await streamReader.ReadToEndAsync();
            var credentialData = JObject.Parse(data);
            var token = await _authenticationProvider.AuthenticateAsync(credentialData);
            if (token == null) return new NotFoundResult();

            return Ok(new AuthenticationDto
            {
                access_token = token.Token,
                expires_in = token.ExpiresIn,
                token_type = token.TokenType
            });
        }
    }
}
