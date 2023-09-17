using IdentityAuthentication.Extensions;
using IdentityAuthentication.Model;
using IdentityAuthentication.Model.Configurations;
using IdentityAuthentication.Model.Enums;
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
        private readonly AuthenticationConfiguration _authenticationConfiguration;
        private readonly TokenSignatureConfiguration _tokenSignatureConfiguration;

        public ConfigurationController(
            IOptions<AccessTokenConfiguration> accessTokenOption,
            IOptions<RefreshTokenConfiguration> refreshTokenOption,
            IOptions<AuthenticationConfiguration> authenticationOption,
            IOptions<TokenSignatureConfiguration> tokenSignatureOption)
        {
            _accessTokenConfiguration = accessTokenOption.Value;
            _refreshTokenConfiguration = refreshTokenOption.Value;
            _authenticationConfiguration = authenticationOption.Value;

            if (tokenSignatureOption.Value != null)
                _tokenSignatureOption = tokenSignatureOption.Value;
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
                AccessTokenConfiguration = _accessTokenConfiguration,
                AuthenticationConfiguration = _authenticationConfiguration,
                RefreshTokenConfiguration = _refreshTokenConfiguration,
            };

            if (_accessTokenConfiguration.TokenType == TokenType.JWT && _tokenSignatureConfiguration != null && _tokenSignatureConfiguration.IsRsaSignature)
            {
                config.RsaVerifySignatureConfiguration = new RsaVerifySignatureConfiguration
                {
                    RSAKeyType = _tokenSignatureConfiguration.RsaSignature.RSAKeyType,
                    PublicKey = _tokenSignatureConfiguration.RsaSignature.PublicKey,
                    AlgorithmType = _tokenSignatureConfiguration.RsaSignature.AlgorithmType,
                };
            }

            return Ok(config);
        }
    }
}
