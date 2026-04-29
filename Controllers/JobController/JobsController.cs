using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CRUDproject.Dtos.JobDto;
using System.Security.Claims;
using CRUDproject.Services.JobService.Interface;

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

        [HttpGet("GetAllUser")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var userId = GetCurrentUserId(); // Get ID from Token

                // PASS userId to the service
                var items = await _service.GetAllAsync(userId);
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred");
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var userId = GetCurrentUserId();

                // PASS userId so user only sees THEIR job
                var item = await _service.GetByIdAsync(id, userId);
                if (item is null) return NotFound("Job not found or access denied.");

                return Ok(item);
            }
            catch (Exception) { return StatusCode(500, "Error"); }
        }

        [HttpPost("Create")]
        // NOTE: Removed [AllowAnonymous] because you need a token to know WHO is creating the job
        public async Task<IActionResult> Create([FromBody] JobDto dto)
        {
            try
            {
                var userId = GetCurrentUserId();

                // PASS userId so the job gets "Stamped" with the owner
                var created = await _service.CreateAsync(dto, userId);

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

                // PASS userId to ensure only the owner can update
                var updated = await _service.UpdateAsync(id, dto, userId);
                if (updated is null) return NotFound("Update failed: Job not found or unauthorized.");

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

                // PASS userId to ensure only the owner can delete
                var deleted = await _service.DeleteAsync(id, userId);
                if (deleted is null) return NotFound("Delete failed: Job not found or unauthorized.");

                return NoContent();
            }
            catch (Exception) { return StatusCode(500, "Error"); }
        }
    

    }
}
