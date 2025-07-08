using CloudServer.Common.Dto.AuthDto;
using CloudServer.IdentityService.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CloudServer.UI.Controllers
{
    [Route("Account")]
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("Login")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpGet("Register")]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginInputDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.LoginAsync(model);
                if (result != null)
                {
                    return RedirectToAction("Home", "Home");
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine("Model state is valid, proceeding with registration.");
                var result = await _authService.RegisterAsync(model);
                if (result != null)
                {
                    return RedirectToAction("Home", "Home");
                }
                ModelState.AddModelError(string.Empty, "Registration failed.");
            }
            return View(model);
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet("Profile")]
        public async Task<IActionResult> ProfileAsync()
        {
            var user = await _authService.GetProfileAsync();
            return View(user);
        }
    }
}
