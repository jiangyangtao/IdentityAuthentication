using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAuthentication.Application.Controllers
{
    [Authorize]
    [Route("api/v{v:ApiVersion}/[controller]/[action]")]
    [ApiController]
    public abstract class BaseController : Controller
    {
        protected BaseController()
        {
        }
    }
}
