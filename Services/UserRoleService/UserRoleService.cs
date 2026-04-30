using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRUDproject.Dtos.RoleDto;
using CRUDproject.Dtos.UserRoleDto;
using CRUDproject.Reposetries.UserRoleRepo.Interface;
using CRUDproject.Services.UserRoleService.Interface;

namespace CRUDproject.Services.UserRoleService
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;

        public UserRoleService(IUserRoleRepository userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }

        public async Task<IEnumerable<UserRoleResponseDto>> GetAllUserRolesAsync()
        {
            var users = await _userRoleRepository.GetAllUserRolesAsync();
            return users.Select(u => new UserRoleResponseDto
            {
                UserId = u.Id,
                UserName = u.Name,
                Email = u.Email,
                Roles = u.UserRoles?.Select(ur => new RoleDto
                {
                    RoleId = ur.RoleId,
                    RoleName = ur.Role.RoleName
                }).ToList() ?? new List<RoleDto>()
            });
        }

        public async Task<UserRoleResponseDto?> GetUserRolesAsync(int userId)
        {
            var user = await _userRoleRepository.GetUserRolesAsync(userId);
            if (user == null) return null;

            return new UserRoleResponseDto
            {
                UserId = user.Id,
                UserName = user.Name,
                Email = user.Email,
                Roles = user.UserRoles?.Select(ur => new RoleDto
                {
                    RoleId = ur.RoleId,
                    RoleName = ur.Role.RoleName
                }).ToList() ?? new List<RoleDto>()
            };
        }

        public async Task<bool> AssignRoleAsync(AssignRoleDto dto)
        {
            return await _userRoleRepository.AssignRoleAsync(dto.UserId, dto.RoleId);
        }

        public async Task<bool> RemoveRoleAsync(AssignRoleDto dto)
        {
            return await _userRoleRepository.RemoveRoleAsync(dto.UserId, dto.RoleId);
        }
    }
}
