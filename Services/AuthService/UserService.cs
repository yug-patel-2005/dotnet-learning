using CRUDproject.Models.AuthUser;
using CRUDproject.Reposetries.AuthRepo.Interface;
using CRUDproject.Services.AuthService.Interface;
using CRUDproject.Dtos.DTOs;

namespace CRUDproject.Services.AuthService;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserInfoDto> CreateUserAsync(RegisterDto dto)
    {
        bool exists = await _userRepository.EmailExistsAsync(dto.Email);
        if (exists)
            throw new InvalidOperationException("Email is already registered.");

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            Password = dto.Password
        };

        var created = await _userRepository.CreateAsync(user);

        return new UserInfoDto
        {
            Id = created.Id,
            Name = created.Name,
            Email = created.Email,
            CreatedAt = created.CreatedAt
        };
    }


    public async Task<UserInfoDto> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
            throw new KeyNotFoundException($"User with ID {id} not found.");

        return new UserInfoDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        };
    }


    public async Task<UserInfoDto> GetUserByEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);

        if (user is null)
            throw new KeyNotFoundException($"User with email {email} not found.");

        return new UserInfoDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        };
    }


    public async Task<IEnumerable<UserInfoDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllUser();

        return users.Select(u => new UserInfoDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            CreatedAt = u.CreatedAt
        }).ToList();
    }


    public async Task<UserInfoDto> UpdateUserAsync(int id, UpdateUserDto dto)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
            throw new KeyNotFoundException($"User with ID {id} not found.");
        var existingUserWithEmail = await _userRepository.GetByEmailAsync(dto.Email);
        if (existingUserWithEmail is not null && existingUserWithEmail.Id != id)
            throw new InvalidOperationException("Email is already in use.");

        user.Name = dto.Name;
        user.Email = dto.Email;
        if (!string.IsNullOrEmpty(dto.Password))
            user.Password = dto.Password;

        var updated = await _userRepository.UpdateUser(user);

        return new UserInfoDto
        {
            Id = updated.Id,
            Name = updated.Name,
            Email = updated.Email,
            CreatedAt = updated.CreatedAt
        };
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
            throw new KeyNotFoundException($"User with ID {id} not found.");

        await _userRepository.DeleteUser(id);
        return true;
    }
}
