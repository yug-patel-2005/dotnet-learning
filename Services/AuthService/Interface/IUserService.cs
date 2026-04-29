using CRUDproject.Dtos.DTOs;

namespace CRUDproject.Services.AuthService.Interface;

public interface IUserService
{
    Task<UserInfoDto> CreateUserAsync(RegisterDto dto);
    Task<UserInfoDto> GetUserByIdAsync(int id);
    Task<IEnumerable<UserInfoDto>> GetAllUsersAsync();
    Task<UserInfoDto> UpdateUserAsync(int id, UpdateUserDto dto);
    Task<bool> DeleteUserAsync(int id);
}
