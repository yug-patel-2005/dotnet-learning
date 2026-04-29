namespace CRUDproject.Dtos.DTOs;


public class RegisterDto
{
    public int Id { get; set; } 
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}


public class LoginDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class UpdateUserDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Password { get; set; }
}


public class AuthResponseDto
{
    public int Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public string message { get; set; } = string.Empty;

}

public class UserInfoDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class UserValidationDto
{
    public int Id { get; set; }
    public bool IsValid { get; set; }
    public string Message { get; set; } = string.Empty;
    public UserInfoDto? User { get; set; }
}
