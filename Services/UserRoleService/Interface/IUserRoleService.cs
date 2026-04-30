using System.Collections.Generic;
using System.Threading.Tasks;
using CRUDproject.Dtos.UserRoleDto;

namespace CRUDproject.Services.UserRoleService.Interface
{
    public interface IUserRoleService
    {
        Task<IEnumerable<UserRoleResponseDto>> GetAllUserRolesAsync();
        Task<UserRoleResponseDto?> GetUserRolesAsync(int userId);
        Task<bool> AssignRoleAsync(AssignRoleDto dto);
        Task<bool> RemoveRoleAsync(AssignRoleDto dto);
    }
}
