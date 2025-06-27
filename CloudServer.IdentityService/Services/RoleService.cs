using CloudServer.Common.Dto.UserDto;
using CloudServer.IdentityService.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CloudServer.IdentityService.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        public async Task<ICollection<UserRoleDto>> GetAllRolesAsync()
        {
            var roles = await _roleManager.Roles.Select(role => new UserRoleDto
            {
                RoleName = role.Name ?? string.Empty
            }).AsNoTracking().ToListAsync();

            return roles;
        }

        public Task<UserRoleDto?> GetRoleByNameAsync(string roleName)
        {
            var role = _roleManager.Roles
                .Where(r => r.Name == roleName)
                .Select(r => new UserRoleDto
                {
                    RoleName = r.Name ?? string.Empty
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return role;
        }

        public async Task<UserRoleDto> AddRoleAsync(UserRoleDto userRoleDto)
        {
            if (await _roleManager.RoleExistsAsync(userRoleDto.RoleName))
            {
                throw new InvalidOperationException($"Role '{userRoleDto.RoleName}' already exists.");
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(userRoleDto.RoleName));
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Failed to create role '{userRoleDto.RoleName}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            return new UserRoleDto
            {
                RoleName = userRoleDto.RoleName
            };
        }
        public async Task<UserRoleDto> UpdateRoleAsync(string roleName, UserRoleDto userRoleDto)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                throw new InvalidOperationException($"Role '{roleName}' does not exist.");
            }

            var roleExists = await _roleManager.RoleExistsAsync(userRoleDto.RoleName);
            if (roleExists && userRoleDto.RoleName != roleName)
            {
                throw new InvalidOperationException($"Role '{userRoleDto.RoleName}' already exists.");
            }

            role.Name = userRoleDto.RoleName;

            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                throw new InvalidOperationException($"Failed to update role '{roleName}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            return new UserRoleDto
            {
                RoleName = userRoleDto.RoleName
            };
        }

        public Task DeleteRoleAsync(string roleName)
        {
            var role = _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                throw new InvalidOperationException($"Role '{roleName}' does not exist.");
            }

            var result = _roleManager.DeleteAsync(role.Result);
            if (!result.Result.Succeeded)
            {
                throw new InvalidOperationException($"Failed to delete role '{roleName}': {string.Join(", ", result.Result.Errors.Select(e => e.Description))}");
            }

            return Task.CompletedTask;
        }


    }
}
