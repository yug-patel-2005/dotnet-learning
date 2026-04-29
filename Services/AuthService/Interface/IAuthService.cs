using CRUDproject.Dtos.DTOs;

namespace CRUDproject.Services.AuthService.Interface;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
    Task<UserValidationDto> ValidateUserAsync(int userId);
}
