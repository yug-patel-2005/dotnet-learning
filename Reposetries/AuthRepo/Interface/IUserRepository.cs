using CRUDproject.Models.AuthUser;

namespace CRUDproject.Reposetries.AuthRepo.Interface;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(int id);
    Task<User> CreateAsync(User user);
    Task<User> UpdateUser(User user);
    Task<User> DeleteUser(int id);
    Task<IEnumerable<User>> GetAllUser();
    Task<bool> EmailExistsAsync(string email);
    // Inside IUserRepository.cs
    Task AssignRoleAsync(int userId, int roleId);
}
