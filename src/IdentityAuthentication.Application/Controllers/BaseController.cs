using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAuthentication.Application.Controllers
{
    [Authorize]
    [Route("v1/api/[controller]/[action]")]
    [ApiController]
    public abstract class BaseController : Controller
    {
        protected BaseController()
        {
        }
    }
}
