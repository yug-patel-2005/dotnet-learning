using CRUDproject.Dtos.JobDto;
using CRUDproject.Models.AuthUser;
using CRUDproject.Services.JobService.Interface;
using CRUDproject.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CRUDproject.Constants;

namespace CRUDproject.Controllers.JobController
{
    [Authorize]
    [ApiController]
    [Route("api/job")]
    public class JobsController : ControllerBase
    {
        private readonly IJobService _service;
        private readonly ILogger<JobsController> _logger;

        public JobsController(IJobService service, ILogger<JobsController> logger)
        {
            _service = service;
            _logger = logger;
        }

     
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId)
                ? userId
                : 0;
        }

      
        private int GetUserRoleId()
        {
          
            var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;
            return int.TryParse(roleClaim, out var roleId) ? roleId : 0;
        }

        [HttpGet("GetAllUser")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var userId = GetCurrentUserId();
                var roleId = GetUserRoleId(); 

                var items = await _service.GetAllAsync(userId, roleId);
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAll");
                return StatusCode(500, "An error occurred");
            }
        }

   
        [HttpGet("GetByDate")]
        public async Task<IActionResult> GetByDate([FromQuery] DateTime date)
        {
            try
            {
          
                var jobs = await _service.GetJobsByDateAsync(date);

                if (jobs == null || !jobs.Any())
                    return NotFound($"No jobs scheduled for {date.ToShortDateString()}");

                return Ok(jobs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching public jobs by date");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var roleId = GetUserRoleId();

                var item = await _service.GetByIdAsync(id, userId, roleId);
                if (item is null) return NotFound("Job not found or access denied.");

                return Ok(item);
            }
            catch (Exception) { return StatusCode(500, "Error"); }
        }

        [Authorize(Roles = RoleNames.Admin)] 
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] JobDto dto)
        {
            try
            {
                var adminId = GetCurrentUserId();
                var created = await _service.CreateAsync(dto, adminId);

                return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
            }
            catch (Exception) { return StatusCode(500, "Error"); }
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] JobDto dto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var roleId = GetUserRoleId();

                var updated = await _service.UpdateAsync(id, dto, userId, roleId);
                if (updated is null) return NotFound("Update failed: Unauthorized or Not Found.");

                return Ok(updated);
            }
            catch (Exception) { return StatusCode(500, "Error"); }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var roleId = GetUserRoleId();

                var deleted = await _service.DeleteAsync(id, userId, roleId);
                if (deleted is null) return Forbid("Only administrators can delete jobs.");

                return NoContent();
            }
            catch (Exception) { return StatusCode(500, "Error"); }
        }
    }
}