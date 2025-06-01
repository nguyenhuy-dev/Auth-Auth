using JWTAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthentication.Controllers
{
    [ApiController]
    [Route("api/sayhello")]
    public class ApiController : Controller
    {
        //Để authenticated thì nhiệm vụ của client là phải gửi 1 token thông qua 1 header có tên là authorization
        [Authorize]  //phải đăng nhập thành công với 1 token thì mới gọi được hàm này
        [HttpGet]
        public IActionResult SayHello(string? name)
        {
            return Content($"Hello {name}!");
        }

        [HttpGet]
        [Route("byjson")]
        public IEnumerable<HelloContent> SayHelloJson(string? name)
        {
            return [
                new HelloContent
                {
                    Content = $"Hello, {name}!"
                }
            ];
        }
    }
}
