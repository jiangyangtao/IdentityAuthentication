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
        private readonly SecretKeyConfiguration _secretKeyConfiguration;
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
            _secretKeyConfiguration = secretKeyOption.Value;
        }

        [ProducesResponseType(typeof(AuthenticationEndpoints), 200)]
        public IActionResult AuthenticationEndpoints()
        {
            var baseUrl = HttpContext.Request.GetOriginHost();
            var apiV1Path = $"{baseUrl}/api/v{AuthenticationConfigurationDefault.ApiV1.MajorVersion}";

            var endpoints = new AuthenticationEndpoints
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
            var rsaDecryptPrivateKey = _secretKeyConfiguration.RsaDecryptPrivateKey;
            if (_authenticationConfiguration.EnableJwtEncrypt == false) rsaDecryptPrivateKey = string.Empty;

            var config = new IdentityAuthenticationConfiguration
            {
                AccessTokenConfiguration = _accessTokenConfiguration,
                AuthenticationConfiguration = _authenticationConfiguration,
                RefreshTokenConfiguration = _refreshTokenConfiguration,
                SecretKeyConfiguration = new SecretKeyConfigurationBase(_secretKeyConfiguration.HmacSha256Key, _secretKeyConfiguration.RsaSignaturePublicKey, rsaDecryptPrivateKey),
            };
            return Ok(config);
        }
    }
}
