using CRUDproject.Models.AuthUser;
using CRUDproject.Reposetries.AuthRepo.Interface;
using CRUDproject.Services.AuthService.Interface;
using CRUDproject.Dtos.DTOs;

namespace CRUDproject.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public AuthService(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService     = jwtService;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        bool exists = await _userRepository.EmailExistsAsync(dto.Email);
        if (exists)
            throw new InvalidOperationException("Email is already registered.");

        var user = new User
        {
           
            Name     = dto.Name,
            Email    = dto.Email,
            Password = dto.Password   
        };

        var created = await _userRepository.CreateAsync(user);

        await _userRepository.AssignRoleAsync(created.Id, 2);
        var token   = _jwtService.GenerateToken(created);

        return new AuthResponseDto
        {
           
            Id=created.Id,
            Token=token,
            message= "User registered and role assigned successfully."

        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);

        if (user is null || user.Password != dto.Password)
            throw new UnauthorizedAccessException("Invalid email or password.");

        var token = _jwtService.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            message = "Login successful."
        };
    }

    public async Task<UserValidationDto> ValidateUserAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user is null)
        {
            return new UserValidationDto
            {
                IsValid = false,
                Message = "Invalid user: User not found.",
                User = null
            };
        }

        return new UserValidationDto
        {
            IsValid = true,
            Message = "Valid user.",
            User = new UserInfoDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            }
        };


    }


}
