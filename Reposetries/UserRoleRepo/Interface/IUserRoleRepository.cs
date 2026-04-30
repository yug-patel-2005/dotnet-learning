using System.Collections.Generic;
using System.Threading.Tasks;
using CRUDproject.Models.AuthUser;

namespace CRUDproject.Reposetries.UserRoleRepo.Interface
{
    public interface IUserRoleRepository
    {
        Task<IEnumerable<User>> GetAllUserRolesAsync();
        Task<User?> GetUserRolesAsync(int userId);
        Task<bool> AssignRoleAsync(int userId, int roleId);
        Task<bool> RemoveRoleAsync(int userId, int roleId);
    }
}
