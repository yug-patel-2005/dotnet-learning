using CRUDproject.Dtos.JobDto;
using CRUDproject.Models;
using CRUDproject.Reposetries.JobRepository.Interface;
using CRUDproject.Services.JobService.Interface;
using CRUDproject.Enums; // 1. Added the Enums namespace
using System.Data;

namespace CRUDproject.Services.JobService
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _repo;

        public JobService(IJobRepository repo)
        {
            _repo = repo;
        }

        public async Task<JobEntity> CreateAsync(JobDto dto, int currentUserId)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));

            var job = MapToEntity(dto);
            job.CreatedBy = currentUserId;
            job.UserId = dto.UserId;

            return await _repo.CreateAsync(job);
        }

        public async Task<JobEntity?> UpdateAsync(int id, JobDto dto, int currentUserId, int roleId)
        {
            // 2. logic: (roleId == 1) ? 0 : currentUserId
            var targetUserId = (roleId == (int)UserRole.Admin) ? 0 : currentUserId;

            var existing = await _repo.GetByIdAsync(id, targetUserId, roleId);
            if (existing is null) return null;

            UpdateEntityFromDto(existing, dto);

            return await _repo.UpdateAsync(existing, targetUserId, roleId);
        }

        public async Task<IEnumerable<JobEntity>> GetAllAsync(int currentUserId, int roleId)
        {
            // 3. Admin logic using Enum
            if (roleId == (int)UserRole.Admin)
            {
                return await _repo.GetAllAsync(0, roleId);
            }

            return await _repo.GetAllAsync(currentUserId, roleId);
        }

        public async Task<JobEntity?> GetByIdAsync(int id, int currentUserId, int roleId)
        {
            var targetUserId = (roleId == (int)UserRole.Admin) ? 0 : currentUserId;
            return await _repo.GetByIdAsync(id, targetUserId, roleId);
        }

        public async Task<JobEntity?> DeleteAsync(int id, int currentUserId, int roleId)
        {
            var targetUserId = (roleId == (int)UserRole.Admin) ? 0 : currentUserId;
            return await _repo.DeleteAsync(id, targetUserId, roleId);
        }
        public async Task<IEnumerable<JobEntity>> GetJobsByDateAsync(DateTime date)
        {
            return await _repo.GetByDateAsync(date);
        }
        private JobEntity MapToEntity(JobDto dto)
        {
            return new JobEntity
            {
                Division = dto.Division,
                SchoolId = dto.SchoolId,
                PhotographerId = dto.PhotographerId,
                ShootDate = dto.ShootDate,
                EventType = dto.EventType,
                ShootCategory = dto.ShootCategory,
                SaleType = dto.SaleType,
                ShootId = dto.ShootId,
                Status = dto.Status
            };
        }

        private void UpdateEntityFromDto(JobEntity entity, JobDto dto)
        {
            entity.Division = dto.Division;
            entity.SchoolId = dto.SchoolId;
            entity.PhotographerId = dto.PhotographerId;
            entity.ShootDate = dto.ShootDate;
            entity.EventType = dto.EventType;
            entity.ShootCategory = dto.ShootCategory;
            entity.SaleType = dto.SaleType;
            entity.ShootId = dto.ShootId;
            entity.Status = dto.Status;
        }
    }
}