using IdentityAuthentication.Model;
using IdentityAuthentication.Model.Configurations;
using IdentityAuthentication.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IdentityAuthentication.Application.Controllers
{
    [AllowAnonymous]
    public class ConfigurationController : BaseController
    {
        private readonly AccessTokenConfiguration _accessTokenConfiguration;
        private readonly RefreshTokenConfiguration _refreshTokenConfiguration;
        private readonly SecretKeyConfigurationBase _secretKeyConfiguration;
        private readonly AuthenticationConfiguration _authenticationConfiguration;

        public ConfigurationController(
            IOptions<AccessTokenConfiguration> accessTokenOption,
            IOptions<RefreshTokenConfiguration> refreshTokenOption,
            IOptions<SecretKeyConfiguration> secretKeyOption,
            IOptions<AuthenticationConfiguration> authenticationOption)
        {
            _accessTokenConfiguration = accessTokenOption.Value;
            _refreshTokenConfiguration = refreshTokenOption.Value;
            _authenticationConfiguration = authenticationOption.Value;


            var secretKeyConfig = secretKeyOption.Value;
            _secretKeyConfiguration = new SecretKeyConfigurationBase(secretKeyConfig.HmacSha256Key, secretKeyConfig.RsaPublicKey);
        }

        [HttpGet("~/api/[controller]")]
        [ProducesResponseType(typeof(AuthenticationEndpoints), 200)]
        public IActionResult AuthenticationEndpoints()
        {
            var baseUrl = HttpContext.Request.GetOriginHost();
            var apiPath = $"{baseUrl}/v1/api";
            var endpoints = new AuthenticationEndpoints
            {
                AccessTokenConfigurationEndpoint = $"{apiPath}/configuration/AccessTokenConfiguration",
                RefreshTokenConfigurationEndpoint = $"{apiPath}/configuration/RefreshTokenConfiguration",
                SecretKeyConfigurationEndpoint = $"{apiPath}/configuration/SecretKeyConfiguration",
                AutnenticationConfigurationEndpoint = $"{apiPath}/configuration/AuthenticaionConfiguartion",
                GenerateTokenEndpoint = $"{apiPath}/token/generate",
                RefreshToeknEndpoint = $"{apiPath}/token/refresh",
                AuthorizeEndpoint = $"{apiPath}/token/authorize",
                InfoEndpoint = $"{apiPath}/token/info",
            };

            return Ok(endpoints);
        }

        [HttpGet]
        [ProducesResponseType(typeof(AccessTokenConfiguration), 200)]
        public IActionResult AccessTokenConfiguration()
        {
            return Ok(_accessTokenConfiguration);
        }

        [HttpGet]
        [ProducesResponseType(typeof(RefreshTokenConfiguration), 200)]
        public IActionResult RefreshTokenConfiguration()
        {
            return Ok(_refreshTokenConfiguration);
        }

        [HttpGet]
        [ProducesResponseType(typeof(SecretKeyConfigurationBase), 200)]
        public IActionResult SecretKeyConfiguration()
        {
            return Ok(_secretKeyConfiguration);
        }

        [HttpGet]
        [ProducesResponseType(typeof(AuthenticationConfiguration), 200)]
        public IActionResult AuthenticaionConfiguartion()
        {
            return Ok(_authenticationConfiguration);
        }
    }
}
