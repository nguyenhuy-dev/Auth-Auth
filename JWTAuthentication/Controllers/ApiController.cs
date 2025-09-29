using JWTAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthentication.Controllers
{
    [ApiController]
    public class ApiController : Controller
    {
        //Để authenticated thì nhiệm vụ của client là phải gửi 1 token thông qua 1 header có tên là authorization
        [Authorize]  //phải đăng nhập thành công với 1 token thì mới gọi được hàm này
        [HttpGet]
        [Route("api/say-hello")]
        public IActionResult SayHello(string? name)
        {
            return Content($"Hello {name}!");
        }

        [AllowAnonymous] // Off authentication
        [HttpGet]
        [Route("api/return-with-json")]
        public IEnumerable<HelloContent> SayHelloJson(string? name)
        {
            return [
                new HelloContent
                {
                    Content = $"Hello, {name}!"
                }
            ];
        }

        [Authorize(Roles = "Student")]
        [HttpGet]
        [Route("api/konichiwa")]
        public IActionResult Konichiwa(string? name = "Huy")
        {
            return Content($"Konichiwa, {name}!");
        }

        [Authorize(Roles = "Student,Teacher")] // Role1 hoặc Role2
        [HttpGet]
        [Route("api/konichiwa-or")]
        public IActionResult KonichiwaOr(string? name = "Huy")
        {
            return Content($"Konichiwa, {name}!");
        }

        [Authorize(Roles = "Student")] // Role1 và Role2. Truyền Roles trong Payload dạng mảng
        [Authorize(Roles = "Teacher")]
        [HttpGet]
        [Route("api/konichiwa-and")]
        public IActionResult KonichiwaAnd(string? name = "Huy")
        {
            return Content($"Konichiwa, {name}!");
        }
    }
}
