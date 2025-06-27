

using CloudServer.Common.Dto.UserDto;

namespace CloudServer.IdentityService.Interface
{
    public interface IRoleService
    {
        public Task<ICollection<UserRoleDto>> GetAllRolesAsync();
        public Task<UserRoleDto?> GetRoleByNameAsync(string roleName);
        public Task<UserRoleDto> AddRoleAsync(UserRoleDto userRoleDto);
        public Task<UserRoleDto> UpdateRoleAsync(string roleName, UserRoleDto userRoleDto);
        public Task DeleteRoleAsync(string roleName);    }
}
