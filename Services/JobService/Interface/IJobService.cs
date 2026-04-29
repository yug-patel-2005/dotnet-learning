using CRUDproject.Models;
using CRUDproject.Dtos.JobDto;

namespace CRUDproject.Services.JobService.Interface
{
    public interface IJobService
    {
   
        Task<JobEntity> CreateAsync(JobDto dto, int currentUserId);

       
        Task<JobEntity?> UpdateAsync(int id, JobDto dto, int currentUserId);

      
        Task<JobEntity?> DeleteAsync(int id, int currentUserId);

 
        Task<JobEntity?> GetByIdAsync(int id, int currentUserId);

        Task<IEnumerable<JobEntity>> GetAllAsync(int currentUserId);
    }
}