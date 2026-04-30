using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using CRUDproject.Models.AuthUser;
using CRUDproject.Services.AuthService.Interface;

namespace CRUDproject.Services.AuthService;

public class JwtService : IJwtService
{
    private readonly IConfiguration _config;

    public JwtService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(User user)
    {
        var secretKey = _config["JwtSettings:SecretKey"]!;
        var issuer = _config["JwtSettings:Issuer"]!;
        var audience = _config["JwtSettings:Audience"]!;
        var expiry = int.Parse(_config["JwtSettings:ExpiryInMinutes"]!);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Name, user.Name)
    };

  
        if (user.UserRoles != null)
        {
            foreach (var userRole in user.UserRoles)
            {
            
                claims.Add(new Claim(ClaimTypes.Role, userRole.RoleId.ToString()));
            }
        }

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims, 
            expires: DateTime.UtcNow.AddMinutes(expiry),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}