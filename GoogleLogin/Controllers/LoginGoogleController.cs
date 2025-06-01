using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace GoogleLogin.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class LoginGoogleController : Controller
    {
        public async Task Login()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            });
        }

        [HttpGet("[Action]")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(c => new
            {
                c.Issuer,
                c.OriginalIssuer,
                c.Type,
                c.Value
            });
            return Json(claims);
        }
    }
}
