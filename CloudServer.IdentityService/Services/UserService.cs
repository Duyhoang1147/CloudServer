using CloudServer.Common.Dto.UserDto;
using CloudServer.Data.Entity;
using CloudServer.Data.Models;
using CloudServer.IdentityService.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CloudServer.IdentityService.Services
{
    internal class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _httpContext;


        // User Management
        public UserService(UserManager<ApplicationUser> userManager,
                           IHttpContextAccessor hhtpContext,
                           RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _httpContext = hhtpContext ?? throw new ArgumentNullException(nameof(hhtpContext));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.Select(user => new UserDto
                {
                    Id = user.Id,
                    Username = user.UserName ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                }).AsNoTracking().ToListAsync();

            return users;
        }

        public async Task<UserDto> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            var userClaims = await _userManager.GetClaimsAsync(user);

            var userRoles = await _userManager.GetRolesAsync(user);

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber ?? string.Empty,
                Address = userClaims.FirstOrDefault(c => c.Type == "address")?.Value ?? string.Empty,
            };
            return userDto;
        }

        public async Task<UserInputDto> UpdateUserAsync(string userId, UserInputDto userDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            var userClaims = await _userManager.GetClaimsAsync(user);
             // update User
            user.UserName = userDto.Username;
            user.PhoneNumber = userDto.PhoneNumber;

            // update address claim
            await ClaimUpdate(DefaultClaims.Address, userDto.Address, user);

            var result = await _userManager.UpdateAsync(user);
            if(!result.Succeeded)
            {
                throw new Exception($"Failed to update user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            return new UserInputDto
            {
                Username = user.UserName ?? string.Empty,
                PhoneNumber = user.PhoneNumber ?? string.Empty,
                Address = userClaims.FirstOrDefault(c => c.Type == DefaultClaims.Address)?.Value ?? string.Empty
            };
        }

        public async Task DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception($"Failed to delete user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }

        // User Claims
        public async Task<ICollection<UserClaimsDto>> GetUserClaimsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            var userClaims = await _userManager.GetClaimsAsync(user);
            return userClaims.Select(c => new UserClaimsDto
            {
                ClaimType = c.Type,
                ClaimValue = c.Value
            }).ToList();
        }

        public async Task<UserClaimsDto> AddUserClaimsAsync(string userId, UserClaimsDto userClaimDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            var newClaim = new Claim(userClaimDto.ClaimType, userClaimDto.ClaimValue);
            var addResult = await _userManager.AddClaimAsync(user, newClaim);
            if(addResult.Succeeded)
            {
                throw new Exception($"Failed to add claim: {string.Join(", ", addResult.Errors.Select(e => e.Description))}");
            }

            return new UserClaimsDto
            {
                ClaimType = userClaimDto.ClaimType,
                ClaimValue = userClaimDto.ClaimValue
            };
        }

        public async Task<UserClaimsDto> RemoveUserClaimsAsync(string userId, string claimType)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            var userCliams = await _userManager.GetClaimsAsync(user);
            var CliamRemove = userCliams.FirstOrDefault(c => c.Type == claimType);
            if(CliamRemove == null)
            {
                throw new KeyNotFoundException($"Claim with type {claimType} not found for user with ID {userId}.");
            }
            var removeResult = await _userManager.RemoveClaimAsync(user, CliamRemove);
            if (!removeResult.Succeeded)
            {
                throw new Exception($"Failed to remove claim: {string.Join(", ", removeResult.Errors.Select(e => e.Description))}");
            }

            return new UserClaimsDto
            {
                ClaimType = claimType,
                ClaimValue = CliamRemove.Value
            };
        }

        // User Roles
        public async Task<ICollection<UserRoleDto>> GetUserRoleAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }
            var roles = await _userManager.GetRolesAsync(user);
            return roles.Select(role => new UserRoleDto
            {
                RoleName = role
            }).ToList();
        }

        public async Task<UserRoleDto> AddUserRoleAsync(string userId, UserRoleDto userRoleDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            if(!await _roleManager.RoleExistsAsync(userRoleDto.RoleName)
            {
                throw new KeyNotFoundException($"Role {userRoleDto.RoleName} does not exist.");
            }

            var addRoleResult = await _userManager.AddToRoleAsync(user, userRoleDto.RoleName);
            if (!addRoleResult.Succeeded)
            {
                throw new Exception($"Failed to add role: {string.Join(", ", addRoleResult.Errors.Select(e => e.Description))}");
            }

            return new UserRoleDto
            {
                RoleName = userRoleDto.RoleName
            };
        }

        public async Task<UserRoleDto> RemoveUserRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                throw new Exception($"Failed to remove role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            return new UserRoleDto
            {
                RoleName = roleName
            };
        }

        // internal method
        public async Task<bool> ClaimUpdate(string claimType, string claimValue, ApplicationUser user)
        {
            var usreClaims = _userManager.GetClaimsAsync(user);
            var oldClaim = usreClaims.Result.FirstOrDefault(c => c.Type == claimType);
            var newClaim = new Claim(claimType, claimValue);

            if(oldClaim != null)
            {
                var replaceResult = await _userManager.ReplaceClaimAsync(user, oldClaim, newClaim);
                return replaceResult.Succeeded;
            }

            var addResult = await _userManager.AddClaimAsync(user, newClaim);
            return addResult.Succeeded;
        }
    }
}
