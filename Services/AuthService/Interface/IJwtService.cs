using CRUDproject.Models.AuthUser;

namespace CRUDproject.Services.AuthService.Interface;

public interface IJwtService
{
    string GenerateToken(User user);
}
