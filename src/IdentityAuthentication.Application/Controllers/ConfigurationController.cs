using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAuthentication.Application.Controllers
{
    [AllowAnonymous]
    public class ConfigurationController : BaseController
    {
        public ConfigurationController()
        {
        }

        [HttpGet]
        public IActionResult GetAuthenticationEndpoints()
        {
            return Ok();
        }

        [HttpGet]
        public IActionResult GetAccessTokenConfiguration()
        {
            return Ok();
        }

        [HttpGet]
        public IActionResult GetRefreshTokenConfiguration()
        {
            return Ok();
        }

        [HttpGet]
        public IActionResult GetSecretKeyConfiguration()
        {
            return Ok();
        }

        [HttpGet]
        public IActionResult GetAuthenticaionConfiguartion()
        {
            return Ok();
        }
    }
}
