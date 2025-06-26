using CloudServer.Data.Dto.AuthDto;
using CloudServer.Data.Dto.UserDto;

namespace CloudServer.IdentityService.Interface
{
    public interface IAuthService
    {
        public Task<LoginInputDto> LoginAsync(LoginInputDto model);
        public Task<RegisterDto> RegisterAsync(RegisterDto model);
        public Task LogoutAsync();
        public Task<UserDto> GetUserInfoAsync();
    }
}
