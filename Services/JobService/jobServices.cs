using CRUDproject.Models;
using CRUDproject.Dtos.JobDto;
using CRUDproject.Services.JobService.Interface;
using CRUDproject.Reposetries.JobRepository.Interface;

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

            return await _repo.CreateAsync(job);
        }

     
        public async Task<JobEntity?> UpdateAsync(int id, JobDto dto, int currentUserId)
        {
       
            var existing = await _repo.GetByIdAsync(id, currentUserId);
            if (existing is null) return null; 

            UpdateEntityFromDto(existing, dto);

            return await _repo.UpdateAsync(existing, currentUserId);
        }

        public async Task<IEnumerable<JobEntity>> GetAllAsync(int currentUserId)
        {
          
            return await _repo.GetAllAsync(currentUserId);
        }

        public async Task<JobEntity?> GetByIdAsync(int id, int currentUserId)
        {
            return await _repo.GetByIdAsync(id, currentUserId);
        }

        public async Task<JobEntity?> DeleteAsync(int id, int currentUserId)
        {
            return await _repo.DeleteAsync(id, currentUserId);
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