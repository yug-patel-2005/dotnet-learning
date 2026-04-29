using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CRUDproject.Services.AuthService.Interface;
using CRUDproject.Dtos.DTOs;

namespace CRUDproject.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(dto);
           
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning($"Registration failed: {ex.Message}");
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Registration error: {ex.Message}");
            return StatusCode(500, new { message = "An error occurred during registration" });
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(dto);
       
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning($"Login failed: {ex.Message}");
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Login error: {ex.Message}");
            return StatusCode(500, new { message = "An error occurred during login" });
        }
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized(new { message = "Invalid user token.", isValid = false });

            var result = await _authService.ValidateUserAsync(userId);
            _logger.LogInformation($"User {userId} retrieved their profile");
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting current user: {ex.Message}");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }
}
