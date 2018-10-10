using Microsoft.AspNetCore.Mvc;

namespace Post.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        public IActionResult Ping() => Ok();
    }
}