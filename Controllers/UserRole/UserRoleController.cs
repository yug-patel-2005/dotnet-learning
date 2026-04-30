using System.Threading.Tasks;
using CRUDproject.Dtos.UserRoleDto;
using CRUDproject.Services.UserRoleService.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRUDproject.Controllers.UserRole
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "1")] 
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;

        public UserRoleController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        [HttpGet("GetAllAssigned")]
        public async Task<IActionResult> GetAllUserRoles()
        {
            var users = await _userRoleService.GetAllUserRolesAsync();
            return Ok(users);
        }

        [HttpGet("GetById{userId}")]
        public async Task<IActionResult> GetUserRoles(int userId)
        {
            var user = await _userRoleService.GetUserRolesAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = $"User with ID {userId} not found." });
            }
            return Ok(user);
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _userRoleService.AssignRoleAsync(dto);
            if (!success)
            {
                return BadRequest(new { message = "Failed to assign role. Ensure user and role exist." });
            }

            return Ok(new { message = "Role assigned successfully." });
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveRole([FromBody] AssignRoleDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _userRoleService.RemoveRoleAsync(dto);
            if (!success)
            {
                return BadRequest(new { message = "Failed to remove role. Ensure user has this role." });
            }

            return Ok(new { message = "Role removed successfully." });
        }
    }
}
