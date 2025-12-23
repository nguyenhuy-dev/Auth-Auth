using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HellosController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [Authorize(Policy = "Policy1")]
        public IActionResult SayHello() => Ok($"Chào các con vợ nhớ!");

        [HttpGet("say-name")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [Authorize(Policy = "Policy4")]
        public IActionResult SayName([FromQuery] string name = "Mai") => Ok($"Lại là '{name}' đây");
    }
}
