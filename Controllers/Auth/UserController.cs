using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CRUDproject.Services.AuthService.Interface;
using System.Security.Claims;
using CRUDproject.Dtos.DTOs;

namespace CRUDproject.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId) 
            ? userId 
            : 0;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] RegisterDto dto)
    {
        try
        {
            var user = await _userService.CreateUserAsync(dto);
            _logger.LogInformation($"User created: {user.Email}");
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning($"User creation failed: {ex.Message}");
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating user: {ex.Message}");
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        try
        {
            var userId = GetCurrentUserId();
            var user = await _userService.GetUserByIdAsync(id);
            _logger.LogInformation($"User {userId} retrieved user {id}");
            return Ok(user);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving user: {ex.Message}");
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto dto)
    {
        try
        {
            var user = await _userService.UpdateUserAsync(id, dto);
            return Ok(user);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            var success = await _userService.DeleteUserAsync(id);
            if (!success)
                return NotFound(new { message = "User not found." });
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}
