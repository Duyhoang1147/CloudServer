using CloudServer.Common.Dto.UserDto;

namespace CloudServer.IdentityService.Interface
{
    public interface IUserService
    {
        // User Management
        public Task<UserDto> GetUserByIdAsync(string userId);
        public Task<List<UserDto>> GetAllUsersAsync();
        public Task<UserInputDto> UpdateUserAsync(string userId, UserInputDto userDto);
        public Task DeleteUserAsync(string userId);

        //User Claims
        public Task<ICollection<UserClaimsDto>> GetUserClaimsAsync(string userId);
        public Task<UserClaimsDto> AddUserClaimsAsync(string userId, UserClaimsDto userClaimDto);
        public Task<UserClaimsDto> RemoveUserClaimsAsync(string userId, string claimType);

        // User Roles
        public Task<ICollection<UserRoleDto>> GetUserRoleAsync(string userId);
        public Task<UserRoleDto> AddUserRoleAsync(string userId, UserRoleDto userRoleDto);
        public Task<UserRoleDto> RemoveUserRoleAsync(string userId, string roleName);
    }
}
