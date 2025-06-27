using CloudServer.Common.Dto.AuthDto;
using CloudServer.Common.Dto.UserDto;
using CloudServer.Data.Entity;
using CloudServer.IdentityService.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace CloudServer.IdentityService.Services
{
    internal class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContext;

        public AuthService(UserManager<ApplicationUser> userManager,
                SignInManager<ApplicationUser> signInManager,
                IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _httpContext = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<LoginInputDto> LoginAsync(LoginInputDto model)
        {

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                return model; // Return the model if login is successful
            }
            else
            {
                throw new Exception("Invalid login attempt");
            }
        }
        public Task<RegisterDto> RegisterAsync(RegisterDto model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email
            };

            var result = _userManager.CreateAsync(user, model.Password).Result;
            if (result.Succeeded)
            {
                return Task.FromResult(model); // Return the model if registration is successful
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Registration failed: {errors}");
            }
        }
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<UserDto> GetProfileAsync()
        {
            var userPrincipal = _httpContext.HttpContext?.User;
            if(userPrincipal == null)
            {
                throw new Exception("User unverified");
            }

            var user = await _userManager.GetUserAsync(userPrincipal);
            if(user == null)
            {
                throw new Exception("User not found");
            }

            var UserDto = new UserDto
            {
                Id = user.Id ?? string.Empty,
                Username = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber ?? string.Empty,
                Address = userPrincipal.FindFirst(c => c.Type == "adress")?.Value ?? string.Empty,
            };

            return UserDto;
        }
    }
}
