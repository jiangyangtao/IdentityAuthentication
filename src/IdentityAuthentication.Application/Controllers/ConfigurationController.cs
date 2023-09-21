using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Extensions;
using IdentityAuthentication.Model;
using IdentityAuthentication.Model.Configurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAuthentication.Application.Controllers
{
    [AllowAnonymous]
    public class ConfigurationController : BaseController
    {
        private readonly IAuthenticationConfigurationProvider _authenticationConfigProvider;

        public ConfigurationController(IAuthenticationConfigurationProvider authenticationConfigProvider)
        {
            _authenticationConfigProvider = authenticationConfigProvider;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IdentityAuthenticationEndpoints), 200)]
        public IActionResult AuthenticationEndpoints()
        {
            var baseUrl = HttpContext.Request.GetOriginHost();
            var apiV1Path = $"{baseUrl}/api/v{AuthenticationConfigurationDefault.ApiV1.MajorVersion}";

            var endpoints = new IdentityAuthenticationEndpoints
            {
                AuthenticationConfigurationEndpoint = $"{apiV1Path}/configuration/getconfiguration",
                GenerateTokenEndpoint = $"{apiV1Path}/token/generate",
                RefreshToeknEndpoint = $"{apiV1Path}/token/refresh",
                AuthorizeEndpoint = $"{apiV1Path}/token/authorize",
            };

            return Ok(endpoints);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IdentityAuthenticationConfiguration), 200)]
        public IActionResult GetConfiguration()
        {
            var config = new IdentityAuthenticationConfiguration
            {
                AccessTokenConfiguration = _authenticationConfigProvider.AccessToken,
                AuthenticationConfiguration = _authenticationConfigProvider.Authentication,
                RefreshTokenConfiguration = _authenticationConfigProvider.RefreshToken,
            };

            if (_authenticationConfigProvider.Authentication.IsJwtAndRsaSignature && _authenticationConfigProvider.RsaSignature != null)
            {
                config.RsaVerifySignatureConfiguration = _authenticationConfigProvider.RsaSignature.BuildRsaVerifySignatureConfiguration();
            }

            return Ok(config);
        }
    }
}
